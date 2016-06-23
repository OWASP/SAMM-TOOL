using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwaspSAMM.DAL
{
    [MetadataTypeAttribute(typeof(AssessmentMetadata))]
    [Serializable]
    public partial class Assessment
    {
        /// <summary>
        /// CalculateScore computes the section score based on the value of the questions
        /// </summary>
        /// <returns>Updates the Section Score data in the Assessment object</returns>
        public Assessment CalculateScore()
        {
            int secScore = 0;                   // Section Score
            bool secPartial = false;            // Section Partial Score
            bool secStop = false;               // Section Stop - indicator to stop incrementing the section score when a partial score is encountered

            // Loop through all the children in the Assessment object
            foreach (var o in this.CategoryDatas)
            {
                foreach (var p in o.SectionDatas)
                {
                    secScore = 0;
                    secPartial = false;
                    secStop = false;
                    foreach (var q in p.GroupDatas)
                    {
                        var yesCount = q.QuestionDatas.Where(a => a.Answer == true).Count();
                        var questionCount = q.QuestionDatas.Count();

                        // GroupScore is tri-state.  If all questions in the group are true, then GroupScore = 1
                        //                           If no questions in the group are true, then GroupScore = 0
                        //                           If some questions in the group are true, then GroupScore = -1 (partial score)
                        q.GroupScore = yesCount == questionCount ? 1 : (yesCount == 0 ? 0 : -1);

                        //  If GroupScore is one and not a partial section score, then increment the section score
                        if (q.GroupScore > 0 && !secStop)
                            secScore++;
                        // If GroupScore is a partial score or no score, then set the indicator to stop incrementing the Section Score
                        if (q.GroupScore <= 0)
                            secStop = true;
                        // If GroupScore is a partial score, then set the partial score indicator to true
                        if (q.GroupScore < 0)
                            secPartial = true;
                        // If the GroupScore is 1 and the Section Stop indicator is true, then set partial to true.  This covers the condition
                        // where all questions in a group are true, but a preceeding group is a partial score.
                        if (q.GroupScore > 0 && secStop)
                            secPartial = true;
                    }
                    p.SectionScore = secScore;
                    p.SectionScorePartial = secPartial ? 1 : 0;
                }
            }
            return this;
        }

        public bool IsFinal ()
        {
            return this.Finalized.HasValue ? this.Finalized.Value : false;
        }
    }

    public class AssessmentMetadata
    {
        [Display(Name = "Template Version")]
        public int TemplateVersion { get; set; }

        [Display(Name = "Owner")]
        public int OwnerID { get; set; }

        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [Display(Name = "Product Name")]
        [StringLength(50)]
        public string ApplicationName { get; set; }

        [Display(Name = "Updated")]
        public Nullable<System.DateTime> LastUpdated { get; set; }

        [Display(Name = "Updated")]
        public Nullable<int> LastUpdateBy { get; set; }

        [Display(Name = "Created")]
        public Nullable<System.DateTime> CreateDate { get; set; }

        [Display(Name = "Created")]
        public Nullable<int> CreateBy { get; set; }

        [Display(Name = "Target Industry")]
        public Nullable<int> IndustryID { get; set; }

        [Display(Name = "Business Unit")]
        public string BusinessUnit { get; set; }
        
        [Display(Name = "Final")]
        public Nullable<bool> Finalized { get; set; }

    }
}

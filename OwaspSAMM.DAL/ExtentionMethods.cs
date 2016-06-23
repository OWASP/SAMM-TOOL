using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace OwaspSAMM.DAL
{
    public static class ExtentionMethods
    {
        /// <summary>
        /// Load - extention method simplifies the Repository code by defining an extention method that can be used in loading
        /// the data object. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        

        public static IQueryable<Assessment> Fill(this IQueryable<Assessment> query, AssessmentLoadType la = AssessmentLoadType.All)
        {
            switch (la)
            {
                case AssessmentLoadType.All:
                    return query.Include(x => x.UserData)
                                .Include(x => x.UserData1)
                                .Include(x => x.UserData2)
                                .Include(a => a.CategoryDatas.Select(b => b.Category))
                                .Include(a => a.CategoryDatas.Select(b => b.SectionDatas.Select(c => c.Section)))
                                .Include(a => a.CategoryDatas.Select(b => b.SectionDatas.Select(c => c.GroupDatas.Select(d => d.Group))))
                                .Include(a => a.CategoryDatas.Select(b => b.SectionDatas.Select(c => c.GroupDatas.Select(d => d.QuestionDatas.Select(e => e.Question)))))
                                .Include(a => a.Industry);

                // Less data is needed for the Scorecard so don't load all the data
                case AssessmentLoadType.Scorecard:
                    return query.Include(x => x.UserData)
                                .Include(x => x.UserData1)
                                .Include(x => x.UserData2)
                                .Include(a => a.CategoryDatas.Select(b => b.Category))
                                .Include(a => a.CategoryDatas.Select(b => b.SectionDatas.Select(c => c.Section)))
                                .Include(a => a.Industry.IndustryTargets);

                case AssessmentLoadType.TableOnly:
                    return query.Include(x=>x.UserData1);

                case AssessmentLoadType.Detail:
                    return query.Include(x => x.UserData)
                                .Include(x => x.UserData1)
                                .Include(x => x.UserData2)
                                .Include(a => a.Industry.IndustryTargets);

                default:
                    return query.Include(x => x.UserData)
                                .Include(x => x.UserData1)
                                .Include(x => x.UserData2)
                                .Include(a => a.CategoryDatas.Select(b => b.SectionDatas.Select(c => c.Section)))
                                .Include(a => a.CategoryDatas.Select(b => b.SectionDatas.Select(c => c.GroupDatas.Select(d => d.Group))))
                                .Include(a => a.CategoryDatas.Select(b => b.SectionDatas.Select(c => c.GroupDatas.Select(d => d.QuestionDatas.Select(e => e.Question)))))
                                .Include(a => a.CategoryDatas.Select(b => b.Category))
                                .Include(a => a.Industry);
            }

        }

        public static IQueryable<AssessmentTemplate> Fill(this IQueryable<AssessmentTemplate> query)
        {
            return query.Include(a => a.TemplateCategories.Select(b => b.TemplateSections.Select(c => c.TemplateGroups.Select(d => d.TemplateQuestions.Select(e => e.Question)))))
                        .Include(a => a.TemplateCategories.Select(b => b.TemplateSections.Select(c => c.TemplateGroups.Select(d => d.Group))))
                        .Include(a => a.TemplateCategories.Select(b => b.TemplateSections.Select(c => c.Section)))
                        .Include(a => a.TemplateCategories.Select(b => b.Category));


        }

    }
}

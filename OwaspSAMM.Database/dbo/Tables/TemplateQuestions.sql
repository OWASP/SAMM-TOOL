CREATE TABLE [dbo].[TemplateQuestions] (
    [QueID]         INT IDENTITY (1, 1) NOT NULL,
    [QuestionID]    INT NOT NULL,
    [QuestionOrder] INT NOT NULL,
    [GroupID]       INT NOT NULL,
    CONSTRAINT [PK_TemplateQuestions] PRIMARY KEY CLUSTERED ([QueID] ASC),
    CONSTRAINT [FK_TemplateQuestions_Questions] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[Questions] ([QuestionID]),
    CONSTRAINT [FK_TemplateQuestions_TemplateGroups] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[TemplateGroups] ([GroID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID used as primary key for table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TemplateQuestions', @level2type = N'COLUMN', @level2name = N'QueID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign key into Questions table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TemplateQuestions', @level2type = N'COLUMN', @level2name = N'QuestionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Specifies sort order for question within group', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TemplateQuestions', @level2type = N'COLUMN', @level2name = N'QuestionOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign key to TemplateGroup which this question belongs to', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TemplateQuestions', @level2type = N'COLUMN', @level2name = N'GroupID';


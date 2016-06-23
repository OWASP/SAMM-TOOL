CREATE TABLE [dbo].[TemplateCategories] (
    [CatID]         INT IDENTITY (1, 1) NOT NULL,
    [TemplateID]    INT NOT NULL,
    [CategoryID]    INT NOT NULL,
    [CategoryOrder] INT NOT NULL,
    CONSTRAINT [PK_TemplateCategories] PRIMARY KEY CLUSTERED ([CatID] ASC),
    CONSTRAINT [FK_TemplateCategories_AssessmentTemplate] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[AssessmentTemplate] ([TemplateID]),
    CONSTRAINT [FK_TemplateCategories_Categories] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Categories] ([CategoryID])
);


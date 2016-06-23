CREATE TABLE [dbo].[TemplateSections] (
    [SecID]        INT IDENTITY (1, 1) NOT NULL,
    [SectionID]    INT NOT NULL,
    [SectionOrder] INT NOT NULL,
    [CategoryID]   INT NOT NULL,
    CONSTRAINT [PK_TemplateSections] PRIMARY KEY CLUSTERED ([SecID] ASC),
    CONSTRAINT [FK_TemplateSections_Sections] FOREIGN KEY ([SectionID]) REFERENCES [dbo].[Sections] ([SectionID]),
    CONSTRAINT [FK_TemplateSections_TemplateCategories] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[TemplateCategories] ([CatID])
);


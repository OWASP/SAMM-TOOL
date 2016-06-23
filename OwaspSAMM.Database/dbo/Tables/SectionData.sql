CREATE TABLE [dbo].[SectionData] (
    [SecID]               INT IDENTITY (1, 1) NOT NULL,
    [SectionID]           INT NOT NULL,
    [SectionOrder]        INT NOT NULL,
    [CategoryID]          INT NOT NULL,
    [SectionScore]        INT NULL,
    [SectionScorePartial] INT NULL,
    CONSTRAINT [PK_SectionData] PRIMARY KEY CLUSTERED ([SecID] ASC),
    CONSTRAINT [FK_SectionData_CategoryData] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[CategoryData] ([CatID]),
    CONSTRAINT [FK_SectionData_Sections] FOREIGN KEY ([SectionID]) REFERENCES [dbo].[Sections] ([SectionID])
);


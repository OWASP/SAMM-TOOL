CREATE TABLE [dbo].[Sections] (
    [SectionID]   INT          IDENTITY (1, 1) NOT NULL,
    [SectionName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED ([SectionID] ASC)
);


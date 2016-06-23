CREATE TABLE [dbo].[IndustryTarget] (
    [ID]         INT         IDENTITY (1, 1) NOT NULL,
    [CategoryID] INT         NOT NULL,
    [SectionID]  INT         NOT NULL,
    [IndustryID] INT         NOT NULL,
    [Score]      VARCHAR (2) NULL,
    CONSTRAINT [PK_IndustrySectionDefault] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_IndustrySectionDefault_Categories] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Categories] ([CategoryID]),
    CONSTRAINT [FK_IndustrySectionDefault_Industry] FOREIGN KEY ([IndustryID]) REFERENCES [dbo].[Industry] ([IndustryID]),
    CONSTRAINT [FK_IndustrySectionDefault_Sections] FOREIGN KEY ([SectionID]) REFERENCES [dbo].[Sections] ([SectionID]),
    CONSTRAINT [FK_IndustryTarget_IndustryTargetScoreValues1] FOREIGN KEY ([Score]) REFERENCES [dbo].[IndustryTargetScoreValues] ([Score])
);


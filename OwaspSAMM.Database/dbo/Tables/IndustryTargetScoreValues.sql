CREATE TABLE [dbo].[IndustryTargetScoreValues] (
    [ID]    INT         IDENTITY (1, 1) NOT NULL,
    [Score] VARCHAR (2) NOT NULL,
    CONSTRAINT [PK_IndustryTargetScoreValues] PRIMARY KEY CLUSTERED ([Score] ASC)
);


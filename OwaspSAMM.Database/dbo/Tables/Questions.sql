CREATE TABLE [dbo].[Questions] (
    [QuestionID]   INT           IDENTITY (1, 1) NOT NULL,
    [QuestionText] VARCHAR (400) NOT NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED ([QuestionID] ASC)
);


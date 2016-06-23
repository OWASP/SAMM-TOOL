CREATE TABLE [dbo].[QuestionData] (
    [QID]           INT IDENTITY (1, 1) NOT NULL,
    [QuestionID]    INT NOT NULL,
    [QuestionOrder] INT NOT NULL,
    [Answer]        BIT CONSTRAINT [DF_QuestionData_Answer] DEFAULT ((0)) NULL,
    [GroupID]       INT NOT NULL,
    CONSTRAINT [PK_QuestionData] PRIMARY KEY CLUSTERED ([QID] ASC),
    CONSTRAINT [FK_QuestionData_GroupData] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[GroupData] ([GroID]),
    CONSTRAINT [FK_QuestionData_Questions] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[Questions] ([QuestionID])
);


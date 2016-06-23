CREATE TABLE [dbo].[CategoryData] (
    [CatID]         INT IDENTITY (1, 1) NOT NULL,
    [CategoryID]    INT NOT NULL,
    [CategoryOrder] INT NOT NULL,
    [AssessmentID]  INT NOT NULL,
    CONSTRAINT [PK_CategoryData] PRIMARY KEY CLUSTERED ([CatID] ASC),
    CONSTRAINT [FK_CategoryData_Assessment] FOREIGN KEY ([AssessmentID]) REFERENCES [dbo].[Assessment] ([AssessmentID]),
    CONSTRAINT [FK_CategoryData_Categories] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Categories] ([CategoryID])
);


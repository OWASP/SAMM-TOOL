CREATE TABLE [dbo].[GroupData] (
    [GroID]      INT IDENTITY (1, 1) NOT NULL,
    [GroupID]    INT NOT NULL,
    [GroupOrder] INT NOT NULL,
    [SectionID]  INT NOT NULL,
    [GroupScore] INT CONSTRAINT [DF_GroupData_GroupScore] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_GroupData] PRIMARY KEY CLUSTERED ([GroID] ASC),
    CONSTRAINT [FK_GroupData_Groups] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Groups] ([GroupID]),
    CONSTRAINT [FK_GroupData_SectionData] FOREIGN KEY ([SectionID]) REFERENCES [dbo].[SectionData] ([SecID])
);


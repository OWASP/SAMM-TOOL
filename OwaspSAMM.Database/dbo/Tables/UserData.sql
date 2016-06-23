CREATE TABLE [dbo].[UserData] (
    [UserID]        INT           IDENTITY (1, 1) NOT NULL,
    [UserNTID]      VARCHAR (50)  NULL,
    [UserDomain]    VARCHAR (50)  NULL,
    [LastName]      VARCHAR (50)  NULL,
    [FirstName]     VARCHAR (50)  NULL,
    [ManagerID]     VARCHAR (50)  NULL,
    [MgrLastName]   VARCHAR (50)  NULL,
    [MgrFirstName]  VARCHAR (50)  NULL,
    [ManagerEID]    VARCHAR (50)  NULL,
    [OrgName]       VARCHAR (50)  NULL,
    [LastLoginDate] DATETIME2 (7) NULL,
    [Administrator] BIT           NULL,
    [Manager]       BIT           NULL,
    [BusinessUnit]  VARCHAR (50)  NULL,
    [BUOwner]       BIT           NULL,
    CONSTRAINT [PK_UserData_1] PRIMARY KEY CLUSTERED ([UserID] ASC)
);


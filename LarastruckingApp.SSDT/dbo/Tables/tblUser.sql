CREATE TABLE [dbo].[tblUser] (
    [Userid]                INT           IDENTITY (1, 1) NOT NULL,
    [UserName]              VARCHAR (500) NOT NULL,
    [Password]              VARCHAR (500) NULL,
    [FirstName]             VARCHAR (500) NULL,
    [LastName]              VARCHAR (500) NULL,
    [CreatedOn]             DATETIME      CONSTRAINT [DF_tblUser_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             INT           NOT NULL,
    [ModifiedOn]            DATETIME      NULL,
    [ModifiedBy]            INT           NULL,
    [IsActive]              BIT           CONSTRAINT [DF_tblUser_IsActive] DEFAULT ((1)) NOT NULL,
    [IsDeleted]             BIT           CONSTRAINT [DF_tblUser_IsDeleted] DEFAULT ((0)) NOT NULL,
    [GUID]                  VARCHAR (100) NULL,
    [ResetPasswordDateTime] DATETIME      NULL,
    [GuidGenratedDateTime]  DATETIME      NULL,
    [UserType]              VARCHAR (50)  NULL,
    CONSTRAINT [PK__tblUser__1797D0245D323CC4] PRIMARY KEY CLUSTERED ([Userid] ASC)
);


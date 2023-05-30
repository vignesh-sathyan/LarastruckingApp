CREATE TABLE [dbo].[tblDriver] (
    [DriverID]            INT           IDENTITY (1, 1) NOT NULL,
    [UserId]              INT           NULL,
    [FirstName]           VARCHAR (50)  NULL,
    [LastName]            VARCHAR (50)  NULL,
    [CitizenShip]         VARCHAR (100) NULL,
    [EmailId]             VARCHAR (100) NULL,
    [Address1]            VARCHAR (150) NULL,
    [Address2]            VARCHAR (150) NULL,
    [Country]             INT           NOT NULL,
    [State]               INT           NOT NULL,
    [City]                VARCHAR (150) NULL,
    [Phone]               VARCHAR (20)  NULL,
    [CellNumber]          VARCHAR (20)  NULL,
    [BloodGroup]          VARCHAR (5)   NULL,
    [Vehicle]             VARCHAR (50)  NULL,
    [MedicalConditions]   VARCHAR (MAX) NULL,
    [EmergencyContactOne] VARCHAR (20)  NULL,
    [EmergencyPhoneNoOne] VARCHAR (20)  NULL,
    [RelationshipStatus1] VARCHAR (50)  NULL,
    [EmergencyContactTwo] VARCHAR (20)  NULL,
    [EmergencyPhoneNoTwo] VARCHAR (20)  NULL,
    [RelationshipStatus2] VARCHAR (50)  NULL,
    [LeaveFrom]           DATETIME      NULL,
    [LeaveTo]             DATETIME      NULL,
    [IsActive]            BIT           CONSTRAINT [DF__tblDriver__IsAct__123EB7A3] DEFAULT ((1)) NOT NULL,
    [CreatedDate]         DATETIME      CONSTRAINT [DF__tblDriver__Creat__1332DBDC] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]           INT           NOT NULL,
    [ModifiedDate]        DATETIME      NULL,
    [ModifiedBy]          INT           NULL,
    [IsDeleted]           BIT           CONSTRAINT [DF__tblDriver__IsDel__14270015] DEFAULT ((0)) NOT NULL,
    [DOB]                 VARCHAR (100) NULL,
    [ZipCode]             VARCHAR (100) NULL,
    [DriverLicence]       VARCHAR (100) NULL,
    [STANumber]           VARCHAR (100) NULL,
    [FullTime]            BIT           NULL,
    [LanguageId]          INT           NULL,
    [Extension]           VARCHAR (15)  NULL,
    [ExpirationDate]      DATETIME      NULL,
    CONSTRAINT [PK__tblDrive__F1B1CD24BBA17574] PRIMARY KEY CLUSTERED ([DriverID] ASC),
    CONSTRAINT [FK__tblDriver__Count__27F8EE98] FOREIGN KEY ([Country]) REFERENCES [dbo].[tblCountry] ([ID]),
    CONSTRAINT [FK__tblDriver__Count__3C34F16F] FOREIGN KEY ([Country]) REFERENCES [dbo].[tblCountry] ([ID]),
    CONSTRAINT [FK__tblDriver__Langu__73DA2C14] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[tblLanguage] ([LanguageId]),
    CONSTRAINT [FK__tblDriver__State__28ED12D1] FOREIGN KEY ([State]) REFERENCES [dbo].[tblState] ([ID]),
    CONSTRAINT [FK__tblDriver__State__3D2915A8] FOREIGN KEY ([State]) REFERENCES [dbo].[tblState] ([ID]),
    CONSTRAINT [FK_tblDriver_tblUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tblUser] ([Userid])
);
















GO
CREATE NONCLUSTERED INDEX [ix_tblDriver]
    ON [dbo].[tblDriver]([UserId] ASC, [IsDeleted] ASC);


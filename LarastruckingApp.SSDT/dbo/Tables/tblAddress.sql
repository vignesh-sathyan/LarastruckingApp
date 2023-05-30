CREATE TABLE [dbo].[tblAddress] (
    [AddressId]             INT            IDENTITY (1, 1) NOT NULL,
    [AddressTypeId]         INT            NOT NULL,
    [CompanyName]           VARCHAR (100)  NULL,
    [Address1]              NVARCHAR (MAX) NULL,
    [Address2]              NVARCHAR (MAX) NULL,
    [City]                  NVARCHAR (250) NULL,
    [State]                 INT            NULL,
    [Zip]                   NVARCHAR (10)  NULL,
    [Country]               INT            NULL,
    [ContactPerson]         VARCHAR (100)  NULL,
    [Phone]                 VARCHAR (20)   NULL,
    [Extension]             VARCHAR (15)   NULL,
    [AdditionalPhone1]      VARCHAR (20)   NULL,
    [Extension1]            VARCHAR (10)   NULL,
    [AdditionalPhone2]      VARCHAR (20)   NULL,
    [Extension2]            VARCHAR (10)   NULL,
    [Email]                 VARCHAR (100)  NULL,
    [CreatedOn]             DATETIME       NULL,
    [CreatedBy]             INT            NULL,
    [ModifiedOn]            DATETIME       NULL,
    [ModifiedBy]            INT            NULL,
    [IsDeleted]             BIT            CONSTRAINT [DF_tblAddress_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsActive]              BIT            CONSTRAINT [DF_tblAddress_IsActive] DEFAULT ((1)) NOT NULL,
    [Comments]              NVARCHAR (200) NULL,
    [IsAppointmentRequired] BIT            CONSTRAINT [DF_tblAddress_IsAppointmentRequired] DEFAULT ((0)) NOT NULL,
    [Website]               NVARCHAR (100) NULL,
    [CompanyNickname]       VARCHAR (100)  NULL,
    CONSTRAINT [PK__tblAddre__091C2AFB1EC71552] PRIMARY KEY CLUSTERED ([AddressId] ASC)
);


















GO
CREATE NONCLUSTERED INDEX [IXAddress]
    ON [dbo].[tblAddress]([AddressId] ASC, [CompanyName] ASC, [City] ASC, [State] ASC, [Zip] ASC);


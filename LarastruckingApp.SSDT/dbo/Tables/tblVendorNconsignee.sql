CREATE TABLE [dbo].[tblVendorNconsignee] (
    [VendorNconsigneeId]   INT           IDENTITY (1, 1) NOT NULL,
    [VendorNconsigneeName] VARCHAR (100) NULL,
    [Address]              VARCHAR (200) NULL,
    [Country]              INT           NULL,
    [State]                INT           NULL,
    [City]                 VARCHAR (100) NULL,
    [Phone]                VARCHAR (20)  NULL,
    [Fax]                  VARCHAR (50)  NULL,
    [Email]                VARCHAR (100) NULL,
    [Zip]                  VARCHAR (10)  NULL,
    [CreatedOn]            DATETIME      NOT NULL,
    [CreatedBy]            INT           NOT NULL,
    [IsActive]             BIT           CONSTRAINT [DF__tblVendor__IsAct__4FF1D159] DEFAULT ((1)) NOT NULL,
    [ModifyBy]             INT           NULL,
    [ModifyOn]             DATETIME      NULL,
    [IsConsignee]          BIT           CONSTRAINT [DF__tblVendor__IsCon__50E5F592] DEFAULT ((0)) NULL,
    [IsDeleted]            BIT           CONSTRAINT [DF_tblVendorNconsignee_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]            DATETIME      NULL,
    CONSTRAINT [PK__tblVendo__FE2356E1250474D4] PRIMARY KEY CLUSTERED ([VendorNconsigneeId] ASC)
);




GO



CREATE TABLE [dbo].[tblEquipmentDetail] (
    [EDID]                   INT            IDENTITY (1, 1) NOT NULL,
    [Decal]                  INT            NULL,
    [VehicleType]            INT            NOT NULL,
    [Model]                  VARCHAR (100)  NULL,
    [LicencePlate]           VARCHAR (100)  NULL,
    [EquipmentNo]            NVARCHAR (20)  NULL,
    [WDimension]             VARCHAR (50)   NULL,
    [HDimension]             VARCHAR (50)   NULL,
    [LDimension]             VARCHAR (50)   NULL,
    [Make]                   VARCHAR (100)  NULL,
    [VIN]                    VARCHAR (100)  NULL,
    [CubicFeet]              VARCHAR (100)  NULL,
    [Ownedby]                VARCHAR (100)  NULL,
    [Active]                 BIT            CONSTRAINT [DF__tblEquipm__Activ__7D0E9093] DEFAULT ((1)) NOT NULL,
    [Comments]               NVARCHAR (100) NULL,
    [CreatedBy]              INT            CONSTRAINT [DF__tblEquipm__Creat__7E02B4CC] DEFAULT ((1)) NOT NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF__tblEquipm__Creat__7EF6D905] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]             INT            NULL,
    [ModifiedOn]             DATETIME       NULL,
    [IsDeleted]              BIT            CONSTRAINT [DF__tblEquipm__IsDel__7FEAFD3E] DEFAULT ((0)) NOT NULL,
    [LeaseCompanyName]       NVARCHAR (100) NULL,
    [RegistrationExpiration] DATETIME       NULL,
    [Year]                   INT            NULL,
    [Color]                  NVARCHAR (50)  NULL,
    [LeaseStartDate]         DATETIME       NULL,
    [LeaseEndDate]           DATETIME       NULL,
    [RegistrationImageURL]   NVARCHAR (MAX) NULL,
    [RegistrationImageName]  NVARCHAR (100) NULL,
    [InsuranceImageURL]      NVARCHAR (MAX) NULL,
    [InsauranceImageName]    NVARCHAR (100) NULL,
    [IsOutOfService]         BIT            NULL,
    [OutOfServiceStartDate]  DATETIME       NULL,
    [OutOfServiceEndDate]    DATETIME       NULL,
    [MaxLoad]                VARCHAR (500)  NULL,
    [RollerBed]              VARCHAR (50)   NULL,
    [QRCodeNo]               VARCHAR (50)   NULL,
    CONSTRAINT [PK__tblEquip__277517579E438BFB] PRIMARY KEY CLUSTERED ([EDID] ASC),
    FOREIGN KEY ([VehicleType]) REFERENCES [dbo].[tblEquipmentType] ([VehicleTypeID])
);








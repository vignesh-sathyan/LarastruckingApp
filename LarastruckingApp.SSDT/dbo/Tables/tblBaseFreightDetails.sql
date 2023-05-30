CREATE TABLE [dbo].[tblBaseFreightDetails] (
    [BaseFreightDetailId] INT             IDENTITY (1, 1) NOT NULL,
    [PickupLocationId]    INT             NULL,
    [DeliveryLocationId]  INT             NULL,
    [Commodity]           NVARCHAR (100)  NULL,
    [FreightTypeId]       INT             NULL,
    [PricingMethodId]     INT             NULL,
    [MinFee]              DECIMAL (18, 2) NULL,
    [Upto]                DECIMAL (18, 2) NULL,
    [UnitPrice]           DECIMAL (18, 2) NULL,
    [IsActive]            BIT             CONSTRAINT [DF__tblBaseFr__IsAct__0C85DE4D] DEFAULT ((1)) NOT NULL,
    [IsDeleted]           BIT             CONSTRAINT [DF__tblBaseFr__IsDel__0D7A0286] DEFAULT ((0)) NOT NULL,
    [CreatedDate]         DATETIME        CONSTRAINT [DF__tblBaseFr__Creat__0E6E26BF] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]           INT             NOT NULL,
    [ModifiedDate]        DATETIME        NULL,
    [ModifiedBy]          INT             NULL,
    CONSTRAINT [PK__tblBaseF__7589F31095AD4B83] PRIMARY KEY CLUSTERED ([BaseFreightDetailId] ASC),
    CONSTRAINT [FK__tblBaseFr__Creat__1C873BEC] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    CONSTRAINT [FK__tblBaseFr__Creat__2FCF1A8A] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    CONSTRAINT [FK__tblBaseFr__Deliv__1D7B6025] FOREIGN KEY ([DeliveryLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblBaseFr__Deliv__30C33EC3] FOREIGN KEY ([DeliveryLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblBaseFr__Freig__1E6F845E] FOREIGN KEY ([FreightTypeId]) REFERENCES [dbo].[tblFreightType] ([FreightTypeId]),
    CONSTRAINT [FK__tblBaseFr__Freig__31B762FC] FOREIGN KEY ([FreightTypeId]) REFERENCES [dbo].[tblFreightType] ([FreightTypeId]),
    CONSTRAINT [FK__tblBaseFr__Picku__1F63A897] FOREIGN KEY ([PickupLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblBaseFr__Picku__32AB8735] FOREIGN KEY ([PickupLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblBaseFr__Prici__2057CCD0] FOREIGN KEY ([PricingMethodId]) REFERENCES [dbo].[tblPricingMethod] ([PricingMethodId]),
    CONSTRAINT [FK__tblBaseFr__Prici__339FAB6E] FOREIGN KEY ([PricingMethodId]) REFERENCES [dbo].[tblPricingMethod] ([PricingMethodId])
);






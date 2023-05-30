CREATE TABLE [dbo].[tblBaseAddresstest] (
    [BaseAddressId]       INT           IDENTITY (1, 1) NOT NULL,
    [CustomerId]          BIGINT        NOT NULL,
    [BillingAddress1]     VARCHAR (100) NULL,
    [BillingAddress2]     VARCHAR (100) NULL,
    [BillingCity]         VARCHAR (100) NULL,
    [BillingStateId]      INT           NULL,
    [BillingCountryId]    INT           NULL,
    [BillingZipCode]      VARCHAR (10)  NULL,
    [BillingPhoneNumber]  VARCHAR (50)  NULL,
    [BillingFax]          VARCHAR (50)  NULL,
    [BillingEmail]        VARCHAR (50)  NULL,
    [ShippingAddress1]    VARCHAR (100) NULL,
    [ShippingAddress2]    VARCHAR (100) NULL,
    [ShippingCity]        VARCHAR (100) NULL,
    [ShippingStateId]     INT           NULL,
    [ShippingCountryId]   INT           NULL,
    [ShippingZipCode]     VARCHAR (10)  NULL,
    [ShippingPhoneNumber] VARCHAR (50)  NULL,
    [ShippingFax]         VARCHAR (50)  NULL,
    [ShippingEmail]       VARCHAR (50)  NULL,
    [PALAccount]          VARCHAR (50)  NULL
);


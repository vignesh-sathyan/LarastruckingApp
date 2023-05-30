CREATE TABLE [dbo].[tblCustomerContact] (
    [ContactId]        INT           IDENTITY (1, 1) NOT NULL,
    [CustomerId]       BIGINT        NULL,
    [ContactFirstName] VARCHAR (100) NULL,
    [ContactLastName]  VARCHAR (100) NULL,
    [ContactPhone]     VARCHAR (15)  NULL,
    [ContactExtension] VARCHAR (15)  NULL,
    [ContactEmail]     VARCHAR (50)  NULL,
    [ContactTitle]     VARCHAR (50)  NULL,
    CONSTRAINT [PK_tblCustomerContact] PRIMARY KEY CLUSTERED ([ContactId] ASC),
    CONSTRAINT [FK_tblCustomerContact_tblCustomerRegistration] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[tblCustomerRegistration] ([CustomerID]) ON DELETE CASCADE
);


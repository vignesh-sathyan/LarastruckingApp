CREATE TABLE [dbo].[tblTrailerRental] (
    [TrailerRentalId]    INT             IDENTITY (1, 1) NOT NULL,
    [CustomerId]         BIGINT          NULL,
    [TrailerInstruction] VARCHAR (MAX)   NULL,
    [GrandTotal]         DECIMAL (18, 2) NULL,
    [CreatedDate]        DATETIME        NOT NULL,
    [CreatedBy]          INT             NOT NULL,
    [ModifiedDate]       DATETIME        NULL,
    [ModifiedBy]         INT             NULL,
    [IsDeleted]          BIT             DEFAULT ((0)) NULL,
    [DeletedBy]          INT             NULL,
    [DeletedDate]        DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([TrailerRentalId] ASC),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[tblCustomerRegistration] ([CustomerID]),
    FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[tblUser] ([Userid])
);


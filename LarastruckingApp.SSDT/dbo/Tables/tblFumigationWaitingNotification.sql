CREATE TABLE [dbo].[tblFumigationWaitingNotification] (
    [FumiWatingNotificationId] INT           IDENTITY (1, 1) NOT NULL,
    [FumigationId]             INT           NULL,
    [FumigationRoutsId]        INT           NULL,
    [PickupArrivedOn]          DATETIME      NULL,
    [PickupDepartedOn]         DATETIME      NULL,
    [DeliveryArrivedOn]        DATETIME      NULL,
    [DeliveryDepartedOn]       DATETIME      NULL,
    [PickUpLocationId]         INT           NULL,
    [CustomerId]               BIGINT        NULL,
    [EquipmentNo]              VARCHAR (200) NULL,
    [DriverId]                 INT           NULL,
    [IsDelivered]              BIT           NOT NULL,
    [IsEmailSentPWS]           BIT           NOT NULL,
    [IsEmailSentPWE]           BIT           NOT NULL,
    [IsEmailSentDWS]           BIT           NOT NULL,
    [IsEmailSentDWE]           BIT           NOT NULL,
    [DeliveryLocationId]       INT           NULL,
    PRIMARY KEY CLUSTERED ([FumiWatingNotificationId] ASC)
);




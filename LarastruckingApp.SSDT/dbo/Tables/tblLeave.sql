CREATE TABLE [dbo].[tblLeave] (
    [LeaveId]       INT           IDENTITY (1, 1) NOT NULL,
    [DriverId]      INT           NOT NULL,
    [UserId]        INT           NOT NULL,
    [TakenFrom]     DATETIME      NOT NULL,
    [TakenTo]       DATETIME      NOT NULL,
    [Reason]        VARCHAR (500) NOT NULL,
    [LeaveStatusId] INT           NOT NULL,
    [AppliedBy]     INT           NOT NULL,
    [AppliedOn]     DATETIME      NOT NULL,
    [ModifiedBy]    INT           NOT NULL,
    [ModifiedOn]    DATETIME      NOT NULL,
    CONSTRAINT [PK_tblLeave] PRIMARY KEY CLUSTERED ([LeaveId] ASC),
    CONSTRAINT [FK_tblLeave_tblUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tblUser] ([Userid])
);




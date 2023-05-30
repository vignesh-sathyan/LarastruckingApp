CREATE TABLE [dbo].[tblLeaveStatus] (
    [LeaveStatusId] INT          IDENTITY (1, 1) NOT NULL,
    [LeaveStatus]   VARCHAR (50) NULL,
    CONSTRAINT [PK_tblLeaveStatus] PRIMARY KEY CLUSTERED ([LeaveStatusId] ASC)
);


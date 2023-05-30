CREATE TYPE [dbo].[UT_AccessorialCharges] AS TABLE (
    [RouteNo]               INT             NULL,
    [LoadingPerUnit]        DECIMAL (18, 2) NULL,
    [LoadingAmount]         DECIMAL (18, 2) NULL,
    [UnloadingPerUnit]      DECIMAL (18, 2) NULL,
    [UnloadingAmount]       DECIMAL (18, 2) NULL,
    [PalletExchangePerUnit] DECIMAL (18, 2) NULL,
    [PalletExchangeAmount]  DECIMAL (18, 2) NULL,
    [DieselRefuelingAmount] DECIMAL (18, 2) NULL,
    [OvernightAmount]       DECIMAL (18, 2) NULL,
    [SameDayAmount]         DECIMAL (18, 2) NULL,
    [TotalAssessorialFee]   DECIMAL (18, 2) NULL);


CREATE TABLE [dbo].[PaidByUsTransaction] (
    [Id]                   INT           NOT NULL,
    [OriginAmount]         DECIMAL (18)  NULL,
    [OriginalCurrencyCode] VARCHAR (3)   NULL,
    [BillingAmount]        DECIMAL (18)  NULL,
    [BillingCurrencyCode]  VARCHAR (3)   NULL,
    [SupplierAccountNum]   NVARCHAR (20) NULL,
    [SupplierName]         NVARCHAR (60) NULL,
    [BackOfficeCompany]    VARCHAR (10)  NULL,
    [TripNumber]           VARCHAR (20)  NULL,
    [Status]               INT           NULL,
    [CreatedBy]            INT           NULL,
    [CreatedDateTime]      DATETIME      NULL,
    [ModifiedBy]           INT           NULL,
    [ModifiedDateTime]     DATETIME      NULL,
    [ForWho]               NVARCHAR (60) NULL,
    [Item]                 NVARCHAR (60) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
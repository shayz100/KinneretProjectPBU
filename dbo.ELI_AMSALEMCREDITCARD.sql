CREATE TABLE [dbo].[ELI_AMSALEMCREDITCARD] (
    [CreditCardNo]        NVARCHAR (20) NULL,
    [ExpiryDate]          NVARCHAR (5)  NULL,
    [EmplId]              NVARCHAR (20) NULL,
    [Cvv]                 NVARCHAR(3)   NULL,
    [CompanyId]           NVARCHAR (4)  NULL,
    [AccountType]         NVARCHAR (10) NULL,
    [AccountNum]          NVARCHAR (20) NULL,
    [CurrencyCode]        NVARCHAR (3)  NULL,
    [DataAreaId]          NVARCHAR (4)  NOT NULL,
    [RecVersion]          BIGINT        NULL,
    [RecId]               BIGINT        NOT NULL,
    [AMS_BankNumber]      INT           NULL,
    [AMS_EmpGovernmentId] NVARCHAR (10) NULL,
    [Status]              INT           NULL,
    [CreditLine]          INT           NULL,
    CONSTRAINT [PK_AXEliAmsalemMCreditCard] PRIMARY KEY CLUSTERED ([DataAreaId] ASC, [RecId] ASC)
);


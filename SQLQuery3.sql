CREATE TABLE [ELI_AMSALEMCREDITCARD] (
  [CreditCardNo] NVARCHAR (20),
  [ExpiryDate] NVARCHAR (5),
  [EmplId] NVARCHAR (20) ,
  [Cvv] NVARCHAR (3),
  [CompanyId] NVARCHAR (4),
  [AccountType] NVARCHAR (10),
  [AccountNum] NVARCHAR (20),
  [CurrencyCode] NVARCHAR (3),
  [DataAreaId] NVARCHAR (4),
  [RecVersion] BIGINT,
  [RecId] BIGINT,
  [AMS_BankNumber] INT,
  [AMS_EmpGovernmentId] NVARCHAR (10),
  [Status] INT,
  [CreditLine] INT,
  CONSTRAINT [PK_AXEliAmsalemMCreditCard] PRIMARY KEY (DataAreaId,RecId)
);

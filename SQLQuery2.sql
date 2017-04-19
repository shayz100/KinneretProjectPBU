CREATE TABLE [dbo].[PaidByUsCreditCardTransactionLog] (
    [id]                    INT      NOT NULL,
    [CreditCardRecId]       BIT      NULL,
    [Active]                BIT      NULL,
    [PaidByUsTransactionId] INT      NULL,
    [CreatedBy]             INT      NULL,
    [CreatedDateTime]       DATETIME NULL,
    [ModifiedBy]            INT      NULL,
    [ModifiedDateTime]      DATETIME NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_PaidByUsCreditCardTransactionLog_PaidByUsTransaction] FOREIGN KEY ([PaidByUsTransactionId]) REFERENCES [dbo].[PaidByUsTransaction] ([Id])
);
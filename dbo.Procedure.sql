CREATE PROCEDURE UpdateBookQuantitiesAndCreateLoanSummary
AS
BEGIN
    -- Создаем временную таблицу для хранения суммарного количества взятых книг по названию
    CREATE TABLE #LoanSummary (
        LoanID INT,
        BookID INT,
        StudentID INT,
        LoanDate DATETIME,
        DueDate DATETIME,
        ReturnDate DATETIME,
        Returned BIT,
        QuantityTaken INT
    );

    -- Вставляем данные в временную таблицу
    INSERT INTO #LoanSummary (LoanID, BookID, StudentID, LoanDate, DueDate, ReturnDate, Returned, QuantityTaken)
    SELECT
        l.LoanID,
        l.BookID,
        l.StudentID,
        l.LoanDate,
        l.DueDate,
        l.ReturnDate,
        l.Returned,
        1 AS QuantityTaken
    FROM
        Loans l;

    -- Обновляем таблицу Books, уменьшая значение Quantity на рассчитанную сумму
    UPDATE b
    SET b.Quantity = b.Quantity - ls.TotalQuantityTaken
    FROM
        Books b
    INNER JOIN
        (SELECT BookID, SUM(QuantityTaken) AS TotalQuantityTaken
         FROM #LoanSummary
         GROUP BY BookID) ls ON b.BookID = ls.BookID;

    -- Создаем итоговую таблицу для хранения результатов
    IF OBJECT_ID('dbo.LoanSummary', 'U') IS NOT NULL
        DROP TABLE dbo.LoanSummary;

    SELECT
        ls.LoanID,
        ls.BookID,
        ls.StudentID,
        ls.LoanDate,
        ls.DueDate,
        ls.ReturnDate,
        ls.Returned,
        ls.QuantityTaken
    INTO
        dbo.LoanSummary
    FROM
        #LoanSummary ls;

    -- Удаляем временную таблицу
    DROP TABLE #LoanSummary;

    -- Возвращаем данные из итоговой таблицы
    SELECT * FROM dbo.LoanSummary;
END;
GO

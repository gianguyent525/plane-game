CREATE DATABASE PlaneGameDB;
GO
USE PlaneGameDB;
GO

-- 1) Tài khoản
CREATE TABLE dbo.users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password_hash NVARCHAR(255) NOT NULL,  -- nên lưu hash (bcrypt) từ backend
    created_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    is_active BIT NOT NULL DEFAULT 1
);
GO

-- 2) Điểm từng lượt chơi
CREATE TABLE dbo.scores (
    score_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    score INT NOT NULL CHECK (score >= 0),
    created_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_scores_users FOREIGN KEY (user_id) REFERENCES dbo.users(user_id)
);
GO

-- Index để query leaderboard nhanh
CREATE INDEX IX_scores_user_id ON dbo.scores(user_id);
CREATE INDEX IX_scores_score_desc ON dbo.scores(score DESC);
GO

-- View bảng xếp hạng: lấy điểm cao nhất theo từng user
CREATE VIEW dbo.vw_leaderboard AS
SELECT
    u.username,
    MAX(s.score) AS score
FROM dbo.scores s
INNER JOIN dbo.users u ON u.user_id = s.user_id
WHERE u.is_active = 1
GROUP BY u.username;
GO

-- SP Login (MVP)
-- Backend truyền @password_hash đã hash cùng cách lưu (hoặc tạm plain text để test)
CREATE OR ALTER PROCEDURE dbo.sp_get_user_for_login
    @username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT user_id, username, password_hash
    FROM dbo.users
    WHERE username = @username AND is_active = 1;
END;
GO

-- SP Đăng ký tài khoản mới (Register)
CREATE OR ALTER PROCEDURE dbo.sp_register_user
    @username NVARCHAR(50),
    @password_hash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra xem username đã tồn tại trong hệ thống chưa
    IF EXISTS (SELECT 1 FROM dbo.users WHERE username = @username)
    BEGIN
        -- Bắn ra lỗi với mức độ 16 để C# Backend có thể catch được SqlException
        RAISERROR(N'Username already exists', 16, 1);
        RETURN;
    END

    -- Nếu chưa tồn tại, tiến hành tạo tài khoản mới
    INSERT INTO dbo.users (username, password_hash)
    VALUES (@username, @password_hash);
    
    -- (Tùy chọn) Trả về thông tin user vừa tạo nếu cần
    -- SELECT SCOPE_IDENTITY() AS user_id;
END;
GO

-- SP Submit score
CREATE OR ALTER PROCEDURE dbo.sp_submit_score
    @username NVARCHAR(50),
    @score INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @user_id INT;

    SELECT @user_id = user_id
    FROM dbo.users
    WHERE username = @username
      AND is_active = 1;

    IF @user_id IS NULL
    BEGIN
        RAISERROR(N'User not found', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.scores(user_id, score)
    VALUES (@user_id, @score);
END;
GO

-- SP Get top N leaderboard
CREATE OR ALTER PROCEDURE dbo.sp_get_top_scores
    @top INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@top)
        username,
        score
    FROM dbo.vw_leaderboard
    ORDER BY score DESC, username ASC;
END;
GO

-- Seed test data
INSERT INTO dbo.users(username, password_hash)
VALUES (N'player1', N'123456'),
       (N'player2', N'123456'),
       (N'player3', N'123456');

EXEC dbo.sp_submit_score @username = N'player1', @score = 120;
EXEC dbo.sp_submit_score @username = N'player3', @score = 250;
EXEC dbo.sp_submit_score @username = N'player2', @score = 180;

EXEC dbo.sp_get_top_scores @top = 10;
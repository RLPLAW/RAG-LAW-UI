-- =============================================
-- Chat Application Database Tables
-- SQL Server Implementation with Auto-Increment IDs
-- =============================================

-- Users Table
CREATE TABLE Users (
    userId INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(255) NOT NULL UNIQUE,
    createdAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    updatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    isActive BIT NOT NULL DEFAULT 1
);

-- Settings Table
CREATE TABLE Settings (
    settingId INT IDENTITY(1,1) PRIMARY KEY,
    userId INT NOT NULL,
    updatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Settings_Users FOREIGN KEY (userId) REFERENCES Users(userId)
);

-- Conversations Table
CREATE TABLE Conversations (
    conversationId INT IDENTITY(1,1) PRIMARY KEY,
    userId INT NOT NULL,
    title NVARCHAR(500),
    createdAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    updatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    isActive BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT FK_Conversations_Users FOREIGN KEY (userId) REFERENCES Users(userId)
);

-- Messages Table
CREATE TABLE Messages (
    messageId INT IDENTITY(1,1) PRIMARY KEY,
    conversationId INT NOT NULL,
    userMessage NVARCHAR(MAX),
    chatResponse NVARCHAR(MAX),
    timeStamp DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Messages_Conversations FOREIGN KEY (conversationId) REFERENCES Conversations(conversationId)
);

-- Logs Table
CREATE TABLE Logs (
    logId INT IDENTITY(1,1) PRIMARY KEY,
    logMessage NVARCHAR(MAX),
    createdAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    isActive BIT NOT NULL DEFAULT 1
);

-- =============================================
-- Insert Sample Data
-- =============================================

-- Insert Users
INSERT INTO Users (username) VALUES 
('nhattq'),
('admin_user');

-- Insert Settings for each user
INSERT INTO Settings (userId)
SELECT userId FROM Users;

-- Insert Conversations
INSERT INTO Conversations (userId, title) VALUES 
(1, 'Getting Started with AI'),
(1, 'Project Planning Discussion'),
(2, 'Technical Support Questions'),
(2, 'Database Design Help'),
(3, 'General Chat'),
(4, 'Learning SQL Queries');

-- Insert Messages
INSERT INTO Messages (conversationId, userMessage, chatResponse) VALUES 
(1, 'Hello! I''m new to AI and want to learn more.', 'Welcome! I''d be happy to help you learn about AI. What specific area interests you most - machine learning, natural language processing, or something else?'),
(1, 'I''m interested in machine learning for business applications.', 'Great choice! Machine learning has many business applications like customer segmentation, demand forecasting, and fraud detection. Would you like to start with a specific use case?'),
(1, 'Yes, I''d like to learn about customer segmentation.', 'Customer segmentation using ML typically involves clustering algorithms like K-means. You''ll need customer data like demographics, purchase history, and behavior patterns.'),

(3, 'I''m having trouble connecting to the database.', 'I can help with database connection issues. What type of database are you trying to connect to, and what error message are you seeing?'),
(3, 'It''s a SQL Server database and I''m getting a timeout error.', 'SQL Server timeout errors are often related to connection string settings, network issues, or long-running queries. Let''s check your connection timeout settings first.'),

(4, 'How do I design a good database schema?', 'Good database design follows several principles: normalize data to reduce redundancy, use appropriate data types, create proper indexes, and establish clear relationships between tables.'),
(4, 'What about primary keys and foreign keys?', 'Primary keys uniquely identify each record in a table, while foreign keys establish relationships between tables. Every table should have a primary key, and foreign keys maintain referential integrity.'),

(5, 'Hi there!', 'Hello! How can I assist you today?'),
(6, 'Can you help me write a SELECT statement?', 'Of course! What data are you trying to retrieve? A basic SELECT statement follows this pattern: SELECT columns FROM table WHERE conditions.');

-- Insert Logs
INSERT INTO Logs (logMessage) VALUES 
('System startup completed successfully'),
('Database connection established'),
('User john_doe logged in'),
('New conversation created by jane_smith'),
('Database backup completed'),
('User sarah_wilson registered'),
('Message processing completed'),
('System maintenance started'),
('Performance optimization applied'),
('Security scan completed - no issues found');
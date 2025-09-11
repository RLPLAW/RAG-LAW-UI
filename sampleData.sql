-- ==========================
-- Insert fake data for Users
-- ==========================
INSERT INTO Users (username, createdAt, updatedAt, isActive) VALUES
('nhat',   '2025-09-01 10:00:00', '2025-09-05 12:30:00', 1),
('bob',     '2025-09-02 09:15:00', '2025-09-06 11:00:00', 1),
('mon', '2025-09-03 14:45:00', '2025-09-04 15:10:00', 0);

-- ==============================
-- Insert fake data for Settings
-- ==============================
INSERT INTO Settings (userId, updatedAt) VALUES
( 1, '2025-09-05 12:30:00'),
( 2, '2025-09-05 12:30:00'),
(3, '2025-09-05 12:30:00');

-- ===================================
-- Insert fake data for Conversations
-- ===================================
INSERT INTO Conversations (userId, title, createdAt, updatedAt, isActive) VALUES
( 1, 'Chat with RAG Bot',   '2025-09-05 10:05:00', '2025-09-05 11:30:00', 1),
(2, 'Legal Query Session', '2025-09-06 09:20:00', '2025-09-06 10:00:00', 1),
( 1, 'Archived Discussion', '2025-09-02 08:30:00', '2025-09-02 09:00:00', 0);

-- ==============================
-- Insert fake data for Messages
-- ==============================
INSERT INTO Messages (conversationId, userMessage, chatResponse, timeStamp) VALUES
(1, 'Hello, how are you?', 'I’m fine, thanks for asking!', '2025-09-05 10:06:00'),
(1, 'Tell me about RAG.', 'RAG stands for Retrieval-Augmented Generation.', '2025-09-05 10:10:00'),
(2, 'What is contract law?', 'Contract law governs agreements…', '2025-09-06 09:25:00'),
(3, 'Old conversation msg', 'Old response here.', '2025-09-02 08:45:00');

-- ==========================
-- Insert fake data for Logs
-- ==========================
INSERT INTO Logs ( logMessage, createdAt, isActive) VALUES
('User alice logged in',              '2025-09-05 10:00:00', 1),
('Conversation 101 started',          '2025-09-05 10:05:00', 1),
( 'User bob updated settings',         '2025-09-06 09:30:00', 1),
('User charlie account deactivated',  '2025-09-04 15:15:00', 0);
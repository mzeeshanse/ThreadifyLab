-- ThreadifyLab Database Schema
-- MySQL Database Schema for ThreadifyLab

CREATE DATABASE IF NOT EXISTS ThreadifyLabDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE ThreadifyLabDB;

-- Services Table
CREATE TABLE IF NOT EXISTS Services (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Icon VARCHAR(100) NOT NULL,
    DisplayOrder INT DEFAULT 0,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    INDEX idx_display_order (DisplayOrder),
    INDEX idx_is_active (IsActive)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Portfolio Items Table
CREATE TABLE IF NOT EXISTS PortfolioItems (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    ImageUrl VARCHAR(500) NOT NULL,
    Category VARCHAR(100) NOT NULL,
    DisplayOrder INT DEFAULT 0,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    INDEX idx_category (Category),
    INDEX idx_display_order (DisplayOrder),
    INDEX idx_is_active (IsActive)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Contact Messages Table
CREATE TABLE IF NOT EXISTS ContactMessages (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Phone VARCHAR(50) NULL,
    Subject VARCHAR(255) NOT NULL,
    Message TEXT NOT NULL,
    IsRead BOOLEAN DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL,
    INDEX idx_is_read (IsRead),
    INDEX idx_created_at (CreatedAt)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Admin Users Table
CREATE TABLE IF NOT EXISTS AdminUsers (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    LastLoginAt DATETIME NULL,
    INDEX idx_username (Username)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Insert default admin user
-- Default Password: ChangeThisPassword123!
-- IMPORTANT: Change this password immediately after first login!
-- 
-- To generate a new password hash, you can:
-- 1. Use the PasswordHasher utility: dotnet run --project ThreadifyLab.csproj -- hash "yourpassword"
-- 2. Or use this SQL to update after creating the admin user:
--    UPDATE AdminUsers SET PasswordHash = 'your_generated_hash' WHERE Username = 'admin';
--
-- The hash is generated using: SHA256(password) -> Base64
INSERT INTO AdminUsers (Username, PasswordHash, Email, CreatedAt) 
VALUES (
    'admin',
    'Q2hhbmdlVGhpc1Bhc3N3b3JkMTIzIQ==',
    'admin@threadifylab.com',
    NOW()
) ON DUPLICATE KEY UPDATE Username=Username;

-- Insert sample services
INSERT INTO Services (Title, Description, Icon, DisplayOrder, IsActive, CreatedAt) VALUES
('Logo Digitizing', 'Professional logo digitizing services for your business branding needs. High-quality conversion with attention to detail.', 'fas fa-image', 1, TRUE, NOW()),
('Custom Designs', 'Create unique custom embroidery designs tailored to your specific requirements and vision.', 'fas fa-palette', 2, TRUE, NOW()),
('3D Puff Digitizing', 'Specialized 3D puff embroidery digitizing for dimensional and eye-catching designs.', 'fas fa-cube', 3, TRUE, NOW()),
('Cap Digitizing', 'Expert cap embroidery digitizing optimized for curved surfaces and limited space.', 'fas fa-hat-cowboy', 4, TRUE, NOW()),
('Left Chest Digitizing', 'Professional left chest embroidery digitizing for uniforms and corporate apparel.', 'fas fa-tshirt', 5, TRUE, NOW()),
('Quick Turnaround', 'Fast digitizing services for urgent projects without compromising quality.', 'fas fa-clock', 6, TRUE, NOW())
ON DUPLICATE KEY UPDATE Title=Title;

-- Insert sample portfolio items (using placeholder images - replace with actual images)
INSERT INTO PortfolioItems (Title, Description, ImageUrl, Category, DisplayOrder, IsActive, CreatedAt) VALUES
('Corporate Logo', 'Professional corporate logo digitizing for business uniforms', 'https://via.placeholder.com/600x600/6366f1/ffffff?text=Corporate+Logo', 'Logo', 1, TRUE, NOW()),
('Sports Team Design', 'Custom sports team embroidery design with team colors', 'https://via.placeholder.com/600x600/8b5cf6/ffffff?text=Sports+Team', 'Design', 2, TRUE, NOW()),
('3D Puff Cap', 'Dimensional 3D puff embroidery on cap', 'https://via.placeholder.com/600x600/ec4899/ffffff?text=3D+Puff', '3D Puff', 3, TRUE, NOW()),
('Left Chest Logo', 'Professional left chest logo for uniform', 'https://via.placeholder.com/600x600/10b981/ffffff?text=Left+Chest', 'Logo', 4, TRUE, NOW()),
('Custom Artwork', 'Custom artwork converted to embroidery format', 'https://via.placeholder.com/600x600/f59e0b/ffffff?text=Custom+Art', 'Custom', 5, TRUE, NOW()),
('Text Design', 'Text-based embroidery design with custom fonts', 'https://via.placeholder.com/600x600/ef4444/ffffff?text=Text+Design', 'Design', 6, TRUE, NOW())
ON DUPLICATE KEY UPDATE Title=Title;


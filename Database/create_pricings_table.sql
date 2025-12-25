-- Quick SQL script to create Pricings table
-- Run this in your MySQL database

USE threadifylabdb;

CREATE TABLE IF NOT EXISTS Pricings (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ServiceName VARCHAR(255) NOT NULL,
    ServiceType VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Description TEXT NOT NULL,
    Icon VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    DisplayOrder INT DEFAULT 0,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    INDEX idx_category (Category),
    INDEX idx_display_order (DisplayOrder),
    INDEX idx_is_active (IsActive),
    INDEX idx_service_type (ServiceType)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Insert default pricing data
INSERT INTO Pricings (ServiceName, ServiceType, Price, Description, Icon, Category, DisplayOrder, IsActive, CreatedAt) VALUES
-- Embroidery Digitizing
('Cap Logo', 'Normal', 15.00, 'Standard cap logo digitizing with basic complexity', 'fas fa-hat-cowboy', 'EmbroideryDigitizing', 1, TRUE, NOW()),
('Cap Logo', 'Complex', 25.00, 'Advanced designs with multiple colors and details', 'fas fa-hat-cowboy', 'EmbroideryDigitizing', 2, TRUE, NOW()),
('Patches', 'Normal', 20.00, 'Standard patch digitizing with simple designs', 'fas fa-shield', 'EmbroideryDigitizing', 3, TRUE, NOW()),
('Patches', 'Complex', 35.00, 'Intricate patch designs with fine details', 'fas fa-shield', 'EmbroideryDigitizing', 4, TRUE, NOW()),
('Jacket Back', 'Normal', 30.00, 'Standard jacket back logo digitizing', 'fas fa-vest', 'EmbroideryDigitizing', 5, TRUE, NOW()),
('Jacket Back', 'Complex', 50.00, 'Large and detailed jacket back designs', 'fas fa-vest', 'EmbroideryDigitizing', 6, TRUE, NOW()),
('Chest Logo', 'Normal', 18.00, 'Standard chest logo digitizing', 'fas fa-tshirt', 'EmbroideryDigitizing', 7, TRUE, NOW()),
('Chest Logo', 'Complex', 28.00, 'Detailed chest logos with multiple elements', 'fas fa-tshirt', 'EmbroideryDigitizing', 8, TRUE, NOW()),
('Badge Logo', 'Normal', 22.00, 'Standard badge logo digitizing', 'fas fa-medal', 'EmbroideryDigitizing', 9, TRUE, NOW()),
('Badge Logo', 'Complex', 38.00, 'Intricate badge designs with fine details', 'fas fa-medal', 'EmbroideryDigitizing', 10, TRUE, NOW()),
-- Vector Art
('Vector Art Services', 'Logo Vectorization', 25.00, 'Convert raster images to clean, scalable vector graphics', 'fas fa-vector-square', 'VectorArt', 1, TRUE, NOW()),
('Vector Art Services', 'Custom Vector Art', 40.00, 'Professional vector illustrations created from scratch', 'fas fa-vector-square', 'VectorArt', 2, TRUE, NOW()),
('Vector Art Services', 'Vector Art Cleanup', 20.00, 'Clean and optimize existing vector files', 'fas fa-vector-square', 'VectorArt', 3, TRUE, NOW()),
('Vector Art Services', 'Complex Vector Graphics', 60.00, 'Detailed vector graphics with multiple layers and elements', 'fas fa-vector-square', 'VectorArt', 4, TRUE, NOW());


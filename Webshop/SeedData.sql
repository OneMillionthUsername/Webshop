-- SQL Seed Script für den IT-Webshop
-- Befüllt die Datenbank mit Beispieldaten
-- Shop existiert seit 2010

-- ============================================
-- 1. KATEGORIEN
-- ============================================
INSERT INTO Categories (Name, Description) VALUES
('Desktops & PCs', 'Desktop-Computer für professionelle und private Nutzung'),
('Laptops & Notebooks', 'Mobile Computer für unterwegs und Büro'),
('Tablets', 'Tablet-Computer für verschiedene Anwendungen'),
('Smartphones', 'Mobiltelefone und Handys der neuesten Generation'),
('Drucker & Scanner', 'Drucker, Scanner und Multifunktionsgeräte'),
('Zubehör', 'Monitore, Tastaturen, Maus, Kabel und sonstige Zubehöre');

-- ============================================
-- 2. PRODUKTE
-- ============================================

-- Desktops & PCs
INSERT INTO Products (Name, Description, BasePrice, CategoryId) VALUES
('Dell OptiPlex 7090 Desktop', 'Professioneller Desktop-Computer mit Intel Core i7', 899.99, 1),
('HP ProDesk 400 G7', 'Business Desktop mit AMD Ryzen 5 Prozessor', 749.99, 1),
('Lenovo ThinkCentre M90', 'Kompakter Desktop für Büroarbeit', 599.99, 1),
('ASUS Vivobook PN50 Mini PC', 'Extrem kompakter Mini-PC für die Schreibtischoberfläche', 399.99, 1),
('Corsair Carbide Gaming PC', 'High-End Gaming Desktop mit RTX 3080', 1999.99, 1);

-- Laptops & Notebooks
INSERT INTO Products (Name, Description, BasePrice, CategoryId) VALUES
('Dell XPS 13', 'Ultraleichter und dünner Laptop mit Intel Core i7', 1199.99, 2),
('Apple MacBook Pro 14"', 'Premium Laptop mit M1 Pro Chip', 1999.99, 2),
('Lenovo ThinkPad X1', 'Business Laptop mit hoher Akkulaufzeit', 999.99, 2),
('ASUS VivoBook 15', 'Budget Laptop mit AMD Ryzen 5', 499.99, 2),
('MSI GS66 Stealth', 'Gaming Laptop mit NVIDIA RTX 3080', 1799.99, 2);

-- Tablets
INSERT INTO Products (Name, Description, BasePrice, CategoryId) VALUES
('Apple iPad Pro 11"', 'Premium Tablet mit M1 Chip', 799.99, 3),
('Samsung Galaxy Tab S7', 'Android Tablet mit 120Hz Display', 649.99, 3),
('iPad Air', 'Mittklasse Tablet mit A14 Bionic', 499.99, 3),
('Lenovo Tab P11 Pro', 'Budget Android Tablet mit OLED Display', 349.99, 3);

-- Smartphones
INSERT INTO Products (Name, Description, BasePrice, CategoryId) VALUES
('iPhone 15 Pro', 'Flagship Apple Smartphone mit A17 Pro', 1099.99, 4),
('Samsung Galaxy S24 Ultra', 'Top-Modell mit S-Pen und 200MP Kamera', 1299.99, 4),
('Google Pixel 8 Pro', 'AI-fokussiertes Android Phone mit hervorragender Kamera', 899.99, 4),
('OnePlus 12', 'Schnelles Smartphone mit hoher Bildwiederholrate', 699.99, 4),
('iPhone 15', 'Standardmodell iPhone mit A16 Bionic', 799.99, 4);

-- Drucker & Scanner
INSERT INTO Products (Name, Description, BasePrice, CategoryId) VALUES
('Brother HL-L8360CDW', 'Farbdrucker für kleine Büros', 449.99, 5),
('HP LaserJet Pro M404n', 'Monochrom Drucker für höhere Volumen', 349.99, 5),
('Canon imageCLASS MF445dw', 'Multifunktionsdrucker mit Scanner und Kopierer', 649.99, 5),
('Xerox VersaLink C405', 'Professioneller Farbdrucker für Agenturen', 999.99, 5),
('Fujitsu ScanSnap iX1600', 'Dokumenten-Scanner mit hohem Durchsatz', 549.99, 5);

-- Zubehör
INSERT INTO Products (Name, Description, BasePrice, CategoryId) VALUES
('Dell UltraSharp 27" 4K Monitor', 'Premium IPS Monitor mit USB-C Docking', 499.99, 6),
('ASUS ProArt 32" 4K Monitor', 'Professioneller Monitor für Bildbearbeitung', 1299.99, 6),
('Logitech MX Master 3S', 'Premium Wireless Maus für Profis', 99.99, 6),
('Keychron K3 Mechanical Keyboard', 'Mechanische Tastatur mit Wireless Verbindung', 199.99, 6),
('Anker PowerCore 65W USB-C Hub', 'USB-C Hub mit Charging und mehreren Anschlüssen', 79.99, 6);

-- ============================================
-- 3. PRODUKTVARIANTEN
-- ============================================

-- Dell OptiPlex 7090 Desktop - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(1, 'DELL-OPTIPLEX-7090-I5', '{"CPU":"Intel Core i5","RAM":"8GB","SSD":"256GB"}', -200, 12),
(1, 'DELL-OPTIPLEX-7090-I7', '{"CPU":"Intel Core i7","RAM":"16GB","SSD":"512GB"}', 0, 8),
(1, 'DELL-OPTIPLEX-7090-I9', '{"CPU":"Intel Core i9","RAM":"32GB","SSD":"1TB"}', 500, 3);

-- HP ProDesk 400 G7 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(2, 'HP-PRODESK-400-R5', '{"CPU":"AMD Ryzen 5","RAM":"8GB","SSD":"256GB"}', -150, 10),
(2, 'HP-PRODESK-400-R7', '{"CPU":"AMD Ryzen 7","RAM":"16GB","SSD":"512GB"}', 0, 7),
(2, 'HP-PRODESK-400-R9', '{"CPU":"AMD Ryzen 9","RAM":"32GB","SSD":"1TB"}', 400, 2);

-- Lenovo ThinkCentre M90 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(3, 'LENOVO-THINKCENTER-M90-STD', '{"CPU":"Intel Core i5","RAM":"8GB","SSD":"256GB"}', 0, 15),
(3, 'LENOVO-THINKCENTER-M90-PRO', '{"CPU":"Intel Core i7","RAM":"16GB","SSD":"512GB"}', 150, 6);

-- ASUS Vivobook PN50 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(4, 'ASUS-PN50-4GB', '{"CPU":"Ryzen 5","RAM":"4GB","SSD":"128GB"}', 0, 20),
(4, 'ASUS-PN50-8GB', '{"CPU":"Ryzen 7","RAM":"8GB","SSD":"256GB"}', 100, 0); -- Ausverkauft

-- Corsair Carbide Gaming PC - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(5, 'CORSAIR-GAMING-RTX3080-32GB', '{"GPU":"RTX 3080","RAM":"32GB","SSD":"1TB"}', 0, 2),
(5, 'CORSAIR-GAMING-RTX4090-64GB', '{"GPU":"RTX 4090","RAM":"64GB","SSD":"2TB"}', 1500, 0); -- Ausverkauft

-- Dell XPS 13 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(6, 'DELL-XPS13-FHD-I5', '{"Display":"1080p FHD","CPU":"Intel Core i5","RAM":"8GB"}', -300, 5),
(6, 'DELL-XPS13-4K-I7', '{"Display":"4K OLED","CPU":"Intel Core i7","RAM":"16GB"}', 0, 3),
(6, 'DELL-XPS13-4K-I9', '{"Display":"4K OLED","CPU":"Intel Core i9","RAM":"32GB"}', 400, 1);

-- Apple MacBook Pro 14" - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(7, 'MACBOOK-PRO-M1PRO-14-256GB', '{"Chip":"M1 Pro","GPU":"10-Core","RAM":"16GB","Storage":"256GB"}', -200, 4),
(7, 'MACBOOK-PRO-M1PRO-14-512GB', '{"Chip":"M1 Pro","GPU":"16-Core","RAM":"16GB","Storage":"512GB"}', 0, 6),
(7, 'MACBOOK-PRO-M1MAX-14-1TB', '{"Chip":"M1 Max","GPU":"32-Core","RAM":"32GB","Storage":"1TB"}', 400, 2);

-- Lenovo ThinkPad X1 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(8, 'THINKPAD-X1-FHD-I5', '{"Display":"FHD","CPU":"Intel Core i5","RAM":"8GB"}', -150, 8),
(8, 'THINKPAD-X1-FHD-I7', '{"Display":"FHD","CPU":"Intel Core i7","RAM":"16GB"}', 0, 5),
(8, 'THINKPAD-X1-4K-I7', '{"Display":"4K UHD","CPU":"Intel Core i7","RAM":"16GB"}', 200, 2);

-- ASUS VivoBook 15 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(9, 'ASUS-VIVOBOOK-R5-8GB', '{"CPU":"Ryzen 5","RAM":"8GB","SSD":"256GB"}', 0, 25),
(9, 'ASUS-VIVOBOOK-R7-16GB', '{"CPU":"Ryzen 7","RAM":"16GB","SSD":"512GB"}', 150, 12);

-- MSI GS66 Stealth - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(10, 'MSI-GS66-RTX3080-32GB', '{"GPU":"RTX 3080","CPU":"Intel i7","RAM":"32GB","Storage":"1TB"}', 0, 3),
(10, 'MSI-GS66-RTX3090-48GB', '{"GPU":"RTX 3090","CPU":"Intel i9","RAM":"48GB","Storage":"2TB"}', 500, 0); -- Ausverkauft

-- iPad Pro 11" - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(11, 'IPAD-PRO-11-128GB', '{"Storage":"128GB","Cellular":"No"}', 0, 8),
(11, 'IPAD-PRO-11-256GB-WIFI', '{"Storage":"256GB","Cellular":"No"}', 100, 5),
(11, 'IPAD-PRO-11-512GB-CELLULAR', '{"Storage":"512GB","Cellular":"Yes"}', 350, 2);

-- Samsung Galaxy Tab S7 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(12, 'GALAXY-TAB-S7-128GB-WIFI', '{"Storage":"128GB","Connectivity":"WiFi"}', 0, 12),
(12, 'GALAXY-TAB-S7-256GB-LTE', '{"Storage":"256GB","Connectivity":"LTE"}', 150, 4);

-- iPad Air - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(13, 'IPAD-AIR-64GB-WIFI', '{"Storage":"64GB","Cellular":"No"}', -50, 10),
(13, 'IPAD-AIR-256GB-WIFI', '{"Storage":"256GB","Cellular":"No"}', 0, 7),
(13, 'IPAD-AIR-256GB-CELLULAR', '{"Storage":"256GB","Cellular":"Yes"}', 150, 3);

-- Lenovo Tab P11 Pro - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(14, 'LENOVO-TAB-P11-128GB', '{"Storage":"128GB"}', 0, 18),
(14, 'LENOVO-TAB-P11-256GB', '{"Storage":"256GB"}', 100, 8);

-- iPhone 15 Pro - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(15, 'IPHONE15PRO-128GB-BLACK', '{"Storage":"128GB","Color":"Black"}', 0, 6),
(15, 'IPHONE15PRO-256GB-GOLD', '{"Storage":"256GB","Color":"Gold"}', 100, 4),
(15, 'IPHONE15PRO-512GB-TITANIUM', '{"Storage":"512GB","Color":"Titanium"}', 300, 2);

-- Samsung Galaxy S24 Ultra - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(16, 'GALAXY-S24-ULTRA-256GB-PHANTOM', '{"Storage":"256GB","Color":"Phantom Black"}', 0, 5),
(16, 'GALAXY-S24-ULTRA-512GB-CREAM', '{"Storage":"512GB","Color":"Cream"}', 100, 3),
(16, 'GALAXY-S24-ULTRA-1TB-TITANIUM', '{"Storage":"1TB","Color":"Titanium Gray"}', 200, 1);

-- Google Pixel 8 Pro - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(17, 'PIXEL-8PRO-128GB-OBSIDIAN', '{"Storage":"128GB","Color":"Obsidian"}', 0, 8),
(17, 'PIXEL-8PRO-256GB-PORCELAIN', '{"Storage":"256GB","Color":"Porcelain"}', 100, 5);

-- OnePlus 12 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(18, 'ONEPLUS12-256GB-BLACK', '{"Storage":"256GB","Color":"Black"}', 0, 15),
(18, 'ONEPLUS12-512GB-SILVER', '{"Storage":"512GB","Color":"Silver"}', 100, 8);

-- iPhone 15 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(19, 'IPHONE15-128GB-BLACK', '{"Storage":"128GB","Color":"Black"}', 0, 20),
(19, 'IPHONE15-256GB-BLUE', '{"Storage":"256GB","Color":"Blue"}', 100, 12),
(19, 'IPHONE15-512GB-RED', '{"Storage":"512GB","Color":"Red"}', 300, 4);

-- Brother HL-L8360CDW - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(20, 'BROTHER-HL-L8360-STANDARD', '{"Model":"Standard","Toner":"Not Included"}', 0, 6),
(20, 'BROTHER-HL-L8360-BUNDLE', '{"Model":"Bundle","Toner":"4-Pack Included"}', 150, 2);

-- HP LaserJet Pro M404n - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(21, 'HP-M404N-STANDARD', '{"Toner":"Not Included"}', 0, 8),
(21, 'HP-M404N-WITH-TONER', '{"Toner":"2-Pack Included"}', 80, 3);

-- Canon imageCLASS MF445dw - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(22, 'CANON-MF445DW-STANDARD', '{"Toner":"Not Included"}', 0, 4),
(22, 'CANON-MF445DW-BUNDLE', '{"Toner":"Starter Set"}', 100, 1);

-- Xerox VersaLink C405 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(23, 'XEROX-C405-STANDARD', '{"Configuration":"Standard"}', 0, 2);

-- Fujitsu ScanSnap iX1600 - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(24, 'FUJITSU-IX1600-STANDARD', '{"Color":"White"}', 0, 5);

-- Dell UltraSharp 27" 4K Monitor - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(25, 'DELL-ULTRASHARP-27-4K', '{"Resolution":"4K UHD","Connector":"USB-C with Docking"}', 0, 10),
(25, 'DELL-ULTRASHARP-27-BASIC', '{"Resolution":"4K UHD","Connector":"HDMI/DP"}', -100, 5);

-- ASUS ProArt 32" 4K Monitor - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(26, 'ASUS-PROART-32-STANDARD', '{"ColorGamut":"100% Adobe RGB"}', 0, 3);

-- Logitech MX Master 3S - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(27, 'LOGITECH-MX-MASTER-GRAPHITE', '{"Color":"Graphite"}', 0, 20),
(27, 'LOGITECH-MX-MASTER-PALE-GRAY', '{"Color":"Pale Gray"}', 0, 15);

-- Keychron K3 Mechanical Keyboard - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(28, 'KEYCHRON-K3-RED-SWITCH', '{"Switch":"Red Mechanical"}', 0, 12),
(28, 'KEYCHRON-K3-BROWN-SWITCH', '{"Switch":"Brown Mechanical"}', 0, 8);

-- Anker PowerCore 65W Hub - Varianten
INSERT INTO ProductVariants (ProductId, SKU, Attributes, PriceAdjustment, StockQuantity) VALUES
(29, 'ANKER-HUB-STANDARD', '{"Color":"Black"}', 0, 30),
(29, 'ANKER-HUB-WHITE', '{"Color":"White"}', 0, 18);

-- ============================================
-- 4. KUNDEN (Zombie-Namen, international)
-- ============================================
INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber, Address, City, PostalCode, RegistrationDate) VALUES
-- Deutschland
('Rotbert', 'Schmidt', 'rotbert.schmidt@example.de', '+49301234567', 'Hauptstraße 42', 'Berlin', '10115', '2010-03-15'),
('Shamblina', 'Müller', 'shamblina.mueller@example.de', '+49891234567', 'Marienplatz 1', 'München', '80331', '2011-06-22'),
('Brainstin', 'Wagner', 'brainstin.wagner@example.de', '+49211234567', 'Königsallee 52', 'Düsseldorf', '40212', '2012-11-08'),
('Decayden', 'Bauer', 'decayden.bauer@example.de', '+49301234567', 'Unter den Linden 77', 'Berlin', '10117', '2013-02-14'),

-- Großbritannien
('Biterly', 'Johnson', 'biterly.johnson@example.uk', '+441632960001', '221B Baker Street', 'London', 'NW1 6XE', '2010-09-03'),
('Groanathan', 'Smith', 'groanathan.smith@example.uk', '+441632960002', '10 Downing Street', 'London', 'SW1A 2AA', '2012-05-17'),
('Moldison', 'Williams', 'moldison.williams@example.uk', '+441632960003', 'Tower of London', 'London', 'EC3N 4AB', '2014-01-22'),

-- Frankreich
('Putrick', 'Dubois', 'putrick.dubois@example.fr', '+33123456789', 'Rue de la Paix 1', 'Paris', '75000', '2011-08-19'),
('Zombina', 'Martin', 'zombina.martin@example.fr', '+33987654321', 'Avenue des Champs-Élysées', 'Paris', '75008', '2013-03-27'),

-- Spanien
('Morticus', 'García', 'morticus.garcia@example.es', '+34912345678', 'Calle Mayor 1', 'Madrid', '28013', '2012-07-11'),
('Necromia', 'López', 'necromia.lopez@example.es', '+34933456789', 'Paseo de Gracia 1', 'Barcelona', '08007', '2014-09-05'),

-- Niederlande
('Corpsington', 'van der Meer', 'corpsington@example.nl', '+31201234567', 'Dam 1', 'Amsterdam', '1012 JS', '2010-12-01'),
('Ravenna', 'de Vries', 'ravenna.devries@example.nl', '+31302345678', 'Neude 1', 'Utrecht', '3511 CE', '2013-04-30'),

-- USA
('Cadaver', 'Johnson', 'cadaver.johnson@example.us', '+12025551234', '1600 Pennsylvania Avenue', 'Washington DC', '20500', '2011-02-14'),
('Sepulcher', 'Williams', 'sepulcher.williams@example.us', '+12125556789', 'Broadway 1', 'New York', '10007', '2012-10-19'),

-- Kanada
('Morghast', 'Thompson', 'morghast.thompson@example.ca', '+14165551234', 'Parliament Hill', 'Ottawa', 'K1A 0A9', '2014-06-15'),

-- Australien
('Thanatos', 'Brown', 'thanatos.brown@example.au', '+61212345678', 'Macquarie Street 1', 'Sydney', '2000', '2013-11-08'),

-- Japan
('Blight', 'Tanaka', 'blight.tanaka@example.jp', '+81312345678', '1-1 Marunouchi', 'Tokyo', '100-0005', '2015-01-20'),

-- Südkorea
('Charnel', 'Kim', 'charnel.kim@example.kr', '+82212345678', 'Jongno-gu', 'Seoul', '03110', '2014-05-10'),

-- Mexiko
('Pestilence', 'García', 'pestilence.garcia@example.mx', '+525551234567', 'Plaza Mayor 1', 'Mexico City', '06500', '2012-03-22'),

-- Brasilien
('Morticia', 'Silva', 'morticia.silva@example.br', '+551133334444', 'Avenida Paulista 1000', 'São Paulo', '01311-100', '2013-08-14'),

-- Indien
('Ghoulbert', 'Patel', 'ghoulbert.patel@example.in', '+919876543210', 'New Delhi Street', 'New Delhi', '110001', '2014-12-03'),

-- China
('Cryptia', 'Wang', 'cryptia.wang@example.cn', '+8601012345678', 'Changan Avenue', 'Beijing', '100000', '2013-07-25');

-- ============================================
-- 5. BESTELLUNGEN (mit verschiedenen Statüssen)
-- ============================================

-- Kundengruppe Deutschland - aktive Kunden mit verschiedenen Bestellungen
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(1, '2020-03-15 10:30:00', 1399.98, 'user_001'),  -- Rotbert: Desktop + Monitor
(1, '2021-08-22 14:15:00', 1299.99, 'user_001'),  -- Rotbert: Laptop
(1, '2023-02-10 09:45:00', 499.99, 'user_001'),   -- Rotbert: Tablet

(2, '2015-05-20 11:00:00', 1799.98, 'user_002'),  -- Shamblina: 2 Desktops
(2, '2018-11-30 15:30:00', 1999.99, 'user_002'),  -- Shamblina: Gaming Laptop
(2, '2022-06-14 10:20:00', 799.99, 'user_002'),   -- Shamblina: Tablet
(2, '2024-01-12 16:45:00', 1259.98, 'user_002'),  -- Shamblina: Smartphone + Zubehör (MIT 10% RABATT)

(3, '2018-07-08 12:45:00', 1399.98, 'user_003'),  -- Brainstin: Desktop + Drucker
(3, '2020-12-25 09:00:00', 1099.99, 'user_003'),  -- Brainstin: iPhone

(4, '2019-04-10 14:20:00', 2399.98, 'user_004'),  -- Decayden: 2 Laptops
(4, '2021-09-03 10:15:00', 649.99, 'user_004'),   -- Decayden: Tablet
(4, '2024-03-22 13:30:00', 1799.99, 'user_004')  -- Decayden: Gaming Laptop

-- UK Kundengruppe - international mix
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(5, '2017-06-12 10:30:00', 1664.97, 'user_005'),   -- Biterly: Desktop + Monitor + Drucker (MIT 10% RABATT)
(5, '2022-01-18 14:00:00', 1999.99, 'user_005'),   -- Biterly: MacBook
(5, '2024-05-11 11:45:00', 899.99, 'user_005'),    -- Biterly: Smartphone

(6, '2016-03-25 09:15:00', 899.98, 'user_006'),   -- Groanathan: Desktop + Keyboard
(6, '2019-11-08 15:30:00', 999.99, 'user_006'),   -- Groanathan: Laptop
(6, '2023-04-20 10:00:00', 1299.99, 'user_006'),    -- Groanathan: Monitor

(7, '2018-09-14 12:00:00', 3199.97, 'user_007')   -- Moldison: High-end Setup (3 Items)

-- Frankreich
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(8, '2014-11-22 10:45:00', 1399.98, 'user_008'),   -- Putrick: Desktop + Monitor
(8, '2021-05-10 13:20:00', 1999.99, 'user_008'),   -- Putrick: MacBook
(8, '2024-07-03 09:30:00', 1299.99, 'user_008'),    -- Putrick: Smartphone

(9, '2017-02-14 14:00:00', 2499.97, 'user_009'),   -- Zombina: Laptop + Monitor + Tablet
(9, '2023-08-19 10:15:00', 799.99, 'user_009')   -- Zombina: iPad Pro

-- Spanien
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(10, '2016-10-05 11:30:00', 3099.97, 'user_010'),  -- Morticus: 2 Desktops + Monitor
(10, '2020-06-21 15:45:00', 1999.99, 'user_010'),  -- Morticus: Gaming Laptop
(10, '2024-02-28 10:00:00', 649.99, 'user_010'),   -- Morticus: Tablet

(11, '2019-04-30 09:00:00', 1499.98, 'user_011'),  -- Necromia: Laptop + Monitor
(11, '2023-12-10 16:20:00', 799.99, 'user_011')  -- Necromia: iPad Pro

-- Niederlande
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(12, '2013-08-15 12:30:00', 2899.97, 'user_012'),  -- Corpsington: High-end Desktop + Monitor + Printer
(12, '2018-03-22 10:15:00', 1499.99, 'user_012'),  -- Corpsington: Dell XPS
(12, '2021-09-09 14:45:00', 799.99, 'user_012'),   -- Corpsington: Smartphone
(12, '2023-11-20 11:00:00', 299.98, 'user_012'),   -- Corpsington: Keyboard + Mouse

(13, '2015-12-01 15:00:00', 1499.98, 'user_013'),  -- Ravenna: Laptop + Monitor
(13, '2020-05-14 09:30:00', 1099.99, 'user_013'),  -- Ravenna: iPhone
(13, '2022-03-10 13:15:00', 649.99, 'user_013')  -- Ravenna: Tablet

-- USA
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(14, '2010-12-24 10:00:00', 1399.98, 'user_014'),  -- Cadaver: Desktop + Monitor
(14, '2015-07-04 14:30:00', 1999.99, 'user_014'),  -- Cadaver: MacBook
(14, '2018-11-23 09:15:00', 1099.99, 'user_014'),   -- Cadaver: Smartphone
(14, '2021-02-14 15:45:00', 799.99, 'user_014'),   -- Cadaver: Tablet
(14, '2023-09-05 10:30:00', 1299.99, 'user_014'),  -- Cadaver: Gaming Laptop (OPEN ORDER - not fully paid)

(15, '2014-06-15 11:00:00', 2399.97, 'user_015'),  -- Sepulcher: Desktop + Monitor + Drucker
(15, '2017-12-20 13:45:00', 1499.99, 'user_015'),  -- Sepulcher: Laptop
(15, '2020-04-01 10:20:00', 1099.99, 'user_015'),  -- Sepulcher: iPad
(15, '2023-03-17 14:00:00', 379.97, 'user_015');  -- Sepulcher: Zubehör (PENDING DELIVERY)

-- Kanada
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(16, '2018-10-31 12:15:00', 3199.97, 'user_016'),  -- Morghast: Workstation Setup (Desktop + Monitor + Drucker)
(16, '2021-07-01 10:45:00', 1999.99, 'user_016'),  -- Morghast: MacBook
(16, '2023-05-20 15:30:00', 699.99, 'user_016'),   -- Morghast: Smartphone
(16, '2024-08-15 11:00:00', 649.99, 'user_016');   -- Morghast: Tablet (BESTELLT NICHT GELIEFERT)

-- Australien
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(17, '2016-02-29 14:00:00', 1499.98, 'user_017'),  -- Thanatos: Laptop + Monitor
(17, '2019-09-21 10:30:00', 1599.98, 'user_017'),  -- Thanatos: 2 Smartphones
(17, '2022-11-11 13:15:00', 1499.99, 'user_017'),  -- Thanatos: Gaming Laptop
(17, '2024-04-10 09:45:00', 649.99, 'user_017')   -- Thanatos: Tablet

-- Japan
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(18, '2017-03-03 11:30:00', 1399.98, 'user_018'),  -- Blight: Desktop + Monitor
(18, '2020-08-08 15:00:00', 1999.99, 'user_018'),  -- Blight: MacBook
(18, '2023-06-15 10:15:00', 1299.99, 'user_018'),  -- Blight: iPad Pro
(18, '2024-09-22 14:30:00', 299.98, 'user_018')  -- Blight: Zubehör (GEKAUFT NOCH NICHT GESENDET)

-- Südkorea
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(19, '2015-09-09 13:45:00', 2899.97, 'user_019'),  -- Charnel: High-end Setup
(19, '2019-01-21 10:00:00', 1899.99, 'user_019'),  -- Charnel: Laptop
(19, '2022-07-07 14:20:00', 1299.99, 'user_019'),  -- Charnel: Smartphone
(19, '2024-06-01 11:15:00', 649.99, 'user_019')   -- Charnel: Tablet (PROBLEM: REKLAMATION)

-- Mexiko
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(20, '2016-05-05 12:00:00', 1399.98, 'user_020'),  -- Pestilence: Desktop + Monitor
(20, '2019-11-11 15:30:00', 1999.99, 'user_020'),  -- Pestilence: MacBook
(20, '2023-02-14 10:45:00', 1099.99, 'user_020')   -- Pestilence: Smartphone

-- Brasilien
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(21, '2014-07-07 11:15:00', 2599.93, 'user_021'),  -- Morticia: Workstation Setup
(21, '2018-12-12 14:45:00', 999.99, 'user_021'),  -- Morticia: Laptop
(21, '2021-04-22 09:30:00', 1099.99, 'user_021'),  -- Morticia: iPad
(21, '2024-10-01 13:00:00', 299.98, 'user_021')   -- Morticia: Zubehör (OFFENE RECHNUNG)

-- Indien
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(22, '2017-07-17 13:30:00', 1399.98, 'user_022'),  -- Ghoulbert: Desktop + Monitor
(22, '2020-02-02 10:00:00', 1599.99, 'user_022'),  -- Ghoulbert: Gaming Laptop
(22, '2023-08-21 15:45:00', 649.99, 'user_022')   -- Ghoulbert: Tablet

-- China
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, UserId) VALUES
(23, '2015-11-11 14:15:00', 2699.97, 'user_023'),  -- Cryptia: High-end Setup
(23, '2019-06-18 11:30:00', 1999.99, 'user_023'),  -- Cryptia: MacBook
(23, '2022-09-30 10:00:00', 1299.99, 'user_023'),  -- Cryptia: iPhone Pro
(23, '2024-07-14 14:45:00', 799.99, 'user_023');   -- Cryptia: Tablet

-- ============================================
-- 6. ORDER ITEMS (Bestellpositionen)
-- ============================================

-- Order 1: Rotbert - Desktop + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(1, 2, 1, 899.99),   -- Dell OptiPlex 7090 i7
(1, 25, 1, 499.99);  -- Dell Monitor

-- Order 2: Rotbert - Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(2, 9, 1, 1299.99); -- MSI Gaming Laptop

-- Order 3: Rotbert - Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(3, 13, 1, 499.99); -- iPad Air

-- Order 4: Shamblina - 2 Desktops
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(4, 2, 2, 899.99); -- Dell OptiPlex 7090 i7

-- Order 5: Shamblina - Gaming Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(5, 10, 1, 1999.99); -- Corsair Gaming PC

-- Order 6: Shamblina - Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(6, 11, 1, 799.99); -- iPad Pro

-- Order 7: Shamblina - Smartphone + Mouse
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(7, 15, 1, 1299.99), -- iPhone 15 Pro
(7, 27, 1, 99.99);   -- Logitech MX Master

-- Order 8: Brainstin - Desktop + Drucker
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(8, 3, 1, 749.99),   -- HP ProDesk
(8, 22, 1, 649.99);  -- Canon MF445dw

-- Order 9: Brainstin - iPhone
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(9, 19, 1, 1099.99); -- iPhone 15 Pro

-- Order 10: Decayden - 2 Laptops
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(10, 6, 2, 1199.99); -- Dell XPS 13

-- Order 11: Decayden - Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(11, 12, 1, 649.99); -- Samsung Galaxy Tab S7

-- Order 12: Decayden - Gaming Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(12, 10, 1, 1799.99); -- MSI GS66 Stealth

-- Order 13: Biterly - Desktop + Monitor + Drucker
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(13, 2, 1, 899.99),  -- Dell OptiPlex i7
(13, 25, 1, 499.99), -- Dell Monitor
(13, 20, 1, 449.99); -- Brother Printer

-- Order 14: Biterly - MacBook
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(14, 7, 1, 1999.99); -- MacBook Pro

-- Order 15: Biterly - Smartphone
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(15, 17, 1, 899.99); -- Google Pixel 8 Pro

-- Order 16: Groanathan - Desktop + Keyboard
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(16, 1, 1, 699.99),  -- Dell OptiPlex i5
(16, 28, 1, 199.99); -- Keychron Keyboard

-- Order 17: Groanathan - Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(17, 8, 1, 999.99); -- Lenovo ThinkPad X1

-- Order 18: Groanathan - Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(18, 26, 1, 1299.99); -- ASUS ProArt 32"

-- Order 19: Moldison - High-end Setup (3 Items)
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(19, 3, 1, 599.99),   -- Lenovo ThinkCentre
(19, 25, 2, 499.99),  -- 2x Dell Monitor
(19, 23, 1, 999.99);  -- Xerox Printer

-- Order 20: Putrick - Desktop + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(20, 2, 1, 899.99),  -- Dell OptiPlex i7
(20, 25, 1, 499.99); -- Dell Monitor

-- Order 21: Putrick - MacBook
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(21, 7, 1, 1999.99); -- MacBook Pro

-- Order 22: Putrick - Smartphone
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(22, 16, 1, 1299.99); -- Samsung Galaxy S24 Ultra

-- Order 23: Zombina - Laptop + Monitor + Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(23, 6, 1, 1199.99), -- Dell XPS 13
(23, 25, 1, 499.99), -- Dell Monitor
(23, 11, 1, 799.99); -- iPad Pro

-- Order 24: Zombina - iPad Pro
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(24, 11, 1, 799.99); -- iPad Pro

-- Order 25: Morticus - 2 Desktops + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(25, 2, 2, 899.99),  -- 2x Dell OptiPlex i7
(25, 26, 1, 1299.99); -- ASUS ProArt Monitor

-- Order 26: Morticus - Gaming Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(26, 10, 1, 1999.99); -- Corsair Gaming PC

-- Order 27: Morticus - Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(27, 12, 1, 649.99); -- Samsung Galaxy Tab S7

-- Order 28: Necromia - Laptop + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(28, 8, 1, 999.99),  -- Lenovo ThinkPad X1
(28, 25, 1, 499.99); -- Dell Monitor

-- Order 29: Necromia - iPad Pro
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(29, 11, 1, 799.99); -- iPad Pro

-- Order 30: Corpsington - High-end Desktop + Monitor + Printer
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(30, 3, 1, 599.99),   -- Lenovo ThinkCentre
(30, 26, 1, 1299.99), -- ASUS ProArt Monitor
(30, 23, 1, 999.99);  -- Xerox Printer

-- Order 31: Corpsington - Dell XPS
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(31, 6, 1, 1499.99); -- Dell XPS 13 (4K i7)

-- Order 32: Corpsington - Smartphone
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(32, 19, 1, 799.99); -- iPhone 15

-- Order 33: Corpsington - Keyboard + Mouse
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(33, 28, 1, 199.99), -- Keychron Keyboard
(33, 27, 1, 99.99);  -- Logitech MX Master

-- Order 34: Ravenna - Laptop + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(34, 8, 1, 999.99),  -- Lenovo ThinkPad X1
(34, 25, 1, 499.99); -- Dell Monitor

-- Order 35: Ravenna - iPhone
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(35, 19, 1, 1099.99); -- iPhone 15 Pro

-- Order 36: Ravenna - Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(36, 13, 1, 499.99); -- iPad Air

-- Order 37: Cadaver - Desktop + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(37, 2, 1, 899.99),  -- Dell OptiPlex i7
(37, 25, 1, 499.99); -- Dell Monitor

-- Order 38: Cadaver - MacBook
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(38, 7, 1, 1999.99); -- MacBook Pro

-- Order 39: Cadaver - Smartphone
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(39, 15, 1, 1099.99); -- iPhone 15 Pro

-- Order 40: Cadaver - Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(40, 11, 1, 799.99); -- iPad Pro

-- Order 41: Cadaver - Gaming Laptop (OPEN ORDER - not fully paid)
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(41, 10, 1, 1299.99); -- MSI GS66 Stealth

-- Order 42: Sepulcher - Desktop + Monitor + Drucker
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(42, 2, 1, 899.99),  -- Dell OptiPlex i7
(42, 25, 1, 499.99), -- Dell Monitor
(42, 23, 1, 999.99); -- Xerox Printer

-- Order 43: Sepulcher - Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(43, 6, 1, 1499.99); -- Dell XPS 13 4K

-- Order 44: Sepulcher - iPad
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(44, 11, 1, 1099.99); -- iPad Pro

-- Order 45: Sepulcher - Zubehör (PENDING DELIVERY)
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(45, 28, 1, 199.99), -- Keychron Keyboard
(45, 27, 1, 99.99),  -- Logitech MX Master
(45, 29, 1, 79.99);  -- Anker USB-C Hub

-- Order 46: Morghast - Workstation Setup
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(46, 3, 1, 599.99),   -- Lenovo ThinkCentre
(46, 26, 2, 1299.99), -- 2x ASUS ProArt Monitors
(46, 21, 1, 349.99);  -- HP Printer

-- Order 47: Morghast - MacBook
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(47, 7, 1, 1999.99); -- MacBook Pro

-- Order 48: Morghast - Smartphone
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(48, 18, 1, 699.99); -- OnePlus 12

-- Order 49: Morghast - Tablet (BESTELLT NICHT GELIEFERT)
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(49, 14, 1, 649.99); -- Lenovo Tab P11 Pro

-- Order 50: Thanatos - Laptop + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(50, 8, 1, 999.99),  -- Lenovo ThinkPad X1
(50, 25, 1, 499.99); -- Dell Monitor

-- Order 51: Thanatos - 2 Smartphones
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(51, 19, 2, 799.99); -- 2x iPhone 15

-- Order 52: Thanatos - Gaming Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(52, 10, 1, 1499.99); -- MSI GS66 Stealth

-- Order 53: Thanatos - Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(53, 12, 1, 649.99); -- Samsung Galaxy Tab S7

-- Order 54: Blight - Desktop + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(54, 2, 1, 899.99),  -- Dell OptiPlex i7
(54, 25, 1, 499.99); -- Dell Monitor

-- Order 55: Blight - MacBook
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(55, 7, 1, 1999.99); -- MacBook Pro

-- Order 56: Blight - iPad Pro
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(56, 11, 1, 1299.99); -- iPad Pro

-- Order 57: Blight - Zubehör (GEKAUFT NOCH NICHT GESENDET)
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(57, 28, 1, 199.99), -- Keychron Keyboard
(57, 27, 1, 99.99);  -- Logitech MX Master

-- Order 58: Charnel - High-end Setup
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(58, 3, 1, 599.99),   -- Lenovo ThinkCentre
(58, 26, 2, 1299.99), -- 2x ASUS ProArt Monitors
(58, 23, 1, 999.99);  -- Xerox Printer

-- Order 59: Charnel - Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(59, 6, 1, 1899.99); -- Dell XPS 13 4K i7

-- Order 60: Charnel - Smartphone
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(60, 16, 1, 1299.99); -- Samsung Galaxy S24 Ultra

-- Order 61: Charnel - Tablet (PROBLEM: REKLAMATION)
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(61, 11, 1, 799.99); -- iPad Pro

-- Order 62: Pestilence - Desktop + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(62, 2, 1, 899.99),  -- Dell OptiPlex i7
(62, 25, 1, 499.99); -- Dell Monitor

-- Order 63: Pestilence - MacBook
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(63, 7, 1, 1999.99); -- MacBook Pro

-- Order 64: Pestilence - Smartphone
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(64, 15, 1, 1099.99); -- iPhone 15 Pro

-- Order 65: Morticia - Workstation Setup
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(65, 3, 1, 599.99),   -- Lenovo ThinkCentre
(65, 26, 2, 1299.99), -- 2x ASUS ProArt Monitors
(65, 21, 1, 349.99);  -- HP Printer

-- Order 66: Morticia - Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(66, 8, 1, 999.99); -- Lenovo ThinkPad X1

-- Order 67: Morticia - iPad
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(67, 11, 1, 1099.99); -- iPad Pro

-- Order 68: Morticia - Zubehör (OFFENE RECHNUNG)
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(68, 28, 1, 199.99), -- Keychron Keyboard
(68, 27, 1, 99.99);  -- Logitech MX Master

-- Order 69: Ghoulbert - Desktop + Monitor
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(69, 2, 1, 899.99),  -- Dell OptiPlex i7
(69, 25, 1, 499.99); -- Dell Monitor

-- Order 70: Ghoulbert - Gaming Laptop
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(70, 10, 1, 1599.99); -- MSI GS66 Stealth

-- Order 71: Ghoulbert - Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(71, 12, 1, 649.99); -- Samsung Galaxy Tab S7

-- Order 72: Cryptia - High-end Setup
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(72, 3, 1, 599.99),   -- Lenovo ThinkCentre
(72, 26, 2, 1299.99), -- 2x ASUS ProArt Monitors
(72, 23, 1, 999.99);  -- Xerox Printer

-- Order 73: Cryptia - MacBook
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(73, 7, 1, 1999.99); -- MacBook Pro

-- Order 74: Cryptia - iPhone Pro
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(74, 15, 1, 1099.99); -- iPhone 15 Pro

-- Order 75: Cryptia - Tablet
INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtPurchase) VALUES
(75, 11, 1, 799.99); -- iPad Pro

-- ============================================
-- 7. PAYMENTS (Zahlungen - verschiedene Status)
-- ============================================

-- Alte Bestellungen (alle bezahlt)
INSERT INTO Payments (OrderId, Amount, PaymentMethod, Status, TransactionId, PaymentDate) VALUES
(1, 1399.98, 'Stripe', 'Completed', 'stripe_1_001', '2020-03-15 10:45:00'),
(2, 1299.99, 'PayPal', 'Completed', 'paypal_1_002', '2021-08-22 14:30:00'),
(3, 499.99, 'Stripe', 'Completed', 'stripe_1_003', '2023-02-10 10:00:00'),
(4, 1799.98, 'Stripe', 'Completed', 'stripe_1_004', '2015-05-20 11:15:00'),
(5, 1999.99, 'PayPal', 'Completed', 'paypal_1_005', '2018-11-30 15:45:00'),
(6, 799.99, 'Stripe', 'Completed', 'stripe_1_006', '2022-06-14 10:35:00'),
(7, 1259.98, 'Stripe', 'Completed', 'stripe_1_007', '2024-01-12 17:00:00'),
(8, 1399.98, 'Stripe', 'Completed', 'stripe_1_008', '2018-07-08 13:00:00'),
(9, 1099.99, 'PayPal', 'Completed', 'paypal_1_009', '2020-12-25 09:30:00'),
(10, 2399.98, 'Stripe', 'Completed', 'stripe_1_010', '2019-04-10 14:35:00'),
(11, 649.99, 'Stripe', 'Completed', 'stripe_1_011', '2021-09-03 10:30:00'),
(12, 1799.99, 'PayPal', 'Completed', 'paypal_1_012', '2024-03-22 13:45:00'),
(13, 1664.97, 'Stripe', 'Completed', 'stripe_1_013', '2017-06-12 10:45:00'),
(14, 1999.99, 'PayPal', 'Completed', 'paypal_1_014', '2022-01-18 14:15:00'),
(15, 899.99, 'Stripe', 'Completed', 'stripe_1_015', '2024-05-11 12:00:00'),
(16, 899.98, 'Stripe', 'Completed', 'stripe_1_016', '2016-03-25 09:30:00'),
(17, 999.99, 'PayPal', 'Completed', 'paypal_1_017', '2019-11-08 15:45:00'),
(18, 1299.99, 'Stripe', 'Completed', 'stripe_1_018', '2023-04-20 10:15:00'),
(19, 3199.97, 'Stripe', 'Completed', 'stripe_1_019', '2018-09-14 12:15:00'),
(20, 1399.98, 'PayPal', 'Completed', 'paypal_1_020', '2014-11-22 10:00:00'),
(21, 1999.99, 'Stripe', 'Completed', 'stripe_1_021', '2021-05-10 13:35:00'),
(22, 1299.99, 'PayPal', 'Completed', 'paypal_1_022', '2024-07-03 09:45:00'),
(23, 2499.97, 'Stripe', 'Completed', 'stripe_1_023', '2017-02-14 14:15:00'),
(24, 799.99, 'PayPal', 'Completed', 'paypal_1_024', '2023-08-19 10:30:00'),
(25, 3099.97, 'Stripe', 'Completed', 'stripe_1_025', '2016-10-05 11:45:00'),
(26, 1999.99, 'PayPal', 'Completed', 'paypal_1_026', '2020-06-21 16:00:00'),
(27, 649.99, 'Stripe', 'Completed', 'stripe_1_027', '2024-02-28 10:15:00'),
(28, 1499.98, 'Stripe', 'Completed', 'stripe_1_028', '2019-04-30 09:15:00'),
(29, 799.99, 'PayPal', 'Completed', 'paypal_1_029', '2023-12-10 16:35:00'),
(30, 2899.97, 'Stripe', 'Completed', 'stripe_1_030', '2013-08-15 12:45:00'),
(31, 1499.99, 'PayPal', 'Completed', 'paypal_1_031', '2018-03-22 10:30:00'),
(32, 799.99, 'Stripe', 'Completed', 'stripe_1_032', '2021-09-09 15:00:00'),
(33, 299.98, 'PayPal', 'Completed', 'paypal_1_033', '2023-11-20 11:15:00'),
(34, 1499.98, 'Stripe', 'Completed', 'stripe_1_034', '2015-12-01 15:15:00'),
(35, 1099.99, 'PayPal', 'Completed', 'paypal_1_035', '2020-05-14 09:45:00'),
(36, 649.99, 'Stripe', 'Completed', 'stripe_1_036', '2022-03-10 13:30:00'),
(37, 1399.98, 'PayPal', 'Completed', 'paypal_1_037', '2010-12-24 10:15:00'),
(38, 1999.99, 'Stripe', 'Completed', 'stripe_1_038', '2015-07-04 14:45:00'),
(39, 1099.99, 'PayPal', 'Completed', 'paypal_1_039', '2018-11-23 09:30:00'),
(40, 799.99, 'Stripe', 'Completed', 'stripe_1_040', '2021-02-14 16:00:00'),
(41, 649.98, 'PayPal', 'Pending', 'paypal_1_041_pending', '2023-09-05 10:45:00'), -- OFFENE RECHNUNG - NUR TEILWEISE BEZAHLT
(42, 2399.97, 'Stripe', 'Completed', 'stripe_1_042', '2014-06-15 11:15:00'),
(43, 1499.99, 'PayPal', 'Completed', 'paypal_1_043', '2017-12-20 14:00:00'),
(44, 1099.99, 'Stripe', 'Completed', 'stripe_1_044', '2020-04-01 10:35:00'),
(45, 379.97, 'PayPal', 'Pending', 'paypal_1_045_pending', '2023-03-17 14:15:00'), -- NOCH NICHT VERSENDET
(46, 3199.97, 'Stripe', 'Completed', 'stripe_1_046', '2018-10-31 12:30:00'),
(47, 1999.99, 'PayPal', 'Completed', 'paypal_1_047', '2021-07-01 11:00:00'),
(48, 699.99, 'Stripe', 'Completed', 'stripe_1_048', '2023-05-20 15:45:00'),
(49, 649.99, 'PayPal', 'Pending', 'paypal_1_049_pending', '2024-08-15 11:15:00'), -- BESTELLT NICHT GELIEFERT
(50, 1499.98, 'Stripe', 'Completed', 'stripe_1_050', '2016-02-29 14:15:00'),
(51, 1599.98, 'PayPal', 'Completed', 'paypal_1_051', '2019-09-21 10:45:00'),
(52, 1499.99, 'Stripe', 'Completed', 'stripe_1_052', '2022-11-11 13:30:00'),
(53, 649.99, 'PayPal', 'Completed', 'paypal_1_053', '2024-04-10 10:00:00'),
(54, 1399.98, 'Stripe', 'Completed', 'stripe_1_054', '2017-03-03 11:45:00'),
(55, 1999.99, 'PayPal', 'Completed', 'paypal_1_055', '2020-08-08 15:15:00'),
(56, 1299.99, 'Stripe', 'Completed', 'stripe_1_056', '2023-06-15 10:30:00'),
(57, 299.98, 'PayPal', 'Pending', 'paypal_1_057_pending', '2024-09-22 14:45:00'), -- GEKAUFT NOCH NICHT GESENDET
(58, 2899.97, 'Stripe', 'Completed', 'stripe_1_058', '2015-09-09 14:00:00'),
(59, 1899.99, 'PayPal', 'Completed', 'paypal_1_059', '2019-01-21 10:15:00'),
(60, 1299.99, 'Stripe', 'Completed', 'stripe_1_060', '2022-07-07 14:35:00'),
(61, 649.99, 'PayPal', 'Pending', 'paypal_1_061_pending', '2024-06-01 11:30:00'), -- REKLAMATION
(62, 1399.98, 'Stripe', 'Completed', 'stripe_1_062', '2016-05-05 12:15:00'),
(63, 1999.99, 'PayPal', 'Completed', 'paypal_1_063', '2019-11-11 15:45:00'),
(64, 1099.99, 'Stripe', 'Completed', 'stripe_1_064', '2023-02-14 11:00:00'),
(65, 2599.93, 'PayPal', 'Completed', 'paypal_1_065', '2014-07-07 11:30:00'),
(66, 999.99, 'Stripe', 'Completed', 'stripe_1_066', '2018-12-12 15:00:00'),
(67, 1099.99, 'PayPal', 'Completed', 'paypal_1_067', '2021-04-22 09:45:00'),
(68, 299.98, 'Stripe', 'Pending', 'stripe_1_068_pending', '2024-10-01 13:15:00'), -- OFFENE RECHNUNG
(69, 1399.98, 'PayPal', 'Completed', 'paypal_1_069', '2017-07-17 13:45:00'),
(70, 1599.99, 'Stripe', 'Completed', 'stripe_1_070', '2020-02-02 10:15:00'),
(71, 649.99, 'PayPal', 'Completed', 'paypal_1_071', '2023-08-21 16:00:00'),
(72, 2699.97, 'Stripe', 'Completed', 'stripe_1_072', '2015-11-11 14:30:00'),
(73, 1999.99, 'PayPal', 'Completed', 'paypal_1_073', '2019-06-18 11:45:00'),
(74, 1299.99, 'Stripe', 'Completed', 'stripe_1_074', '2022-09-30 10:15:00'),
(75, 799.99, 'PayPal', 'Completed', 'paypal_1_075', '2024-07-14 15:00:00');

-- ============================================
-- 8. DISCOUNTS (Rabatte)
-- ============================================

INSERT INTO Discounts (OrderId, Code, Description, DiscountAmount, DiscountPercentage, Type, CreatedDate, Reason) VALUES
(7, 'BULK-DISCOUNT-001', 'Bulk Order Discount', 140.00, 10.00, 'Percentage', '2024-01-12 16:45:00', 'Bulk Order'),
(13, 'LOYALTY-DISCOUNT-001', 'Loyalty Customer Discount', 184.99, 10.00, 'Percentage', '2017-06-12 10:30:00', 'Loyalty');

-- ============================================
-- HINWEIS: 
-- Discounts-Tabelle speichert alle gewährten Rabatte auf Bestellungen
-- Order 7 (Shamblina): TotalAmount 1399.98 - 10% Rabatt (140.00) = 1259.98 ✓
-- Order 13 (Biterly): TotalAmount 1849.97 - 10% Rabatt (184.99) = 1664.98 ✓
-- Mehrere Bestellungen haben offene Zahlungen (Pending Status)
-- Manche Kunden sind inaktiv (letzte Bestellung > 2 Jahre alt)
-- Es gibt verschiedene Lieferstatus:
--   - Gekauft und versendet (alte Orders mit Completed Payment)
--   - Gekauft aber noch nicht versendet (neuere Orders mit Pending)
--   - Offene Rechnungen (Pending Payment)
--   - Reklamationen (Order 61)
-- Stock: Einige Varianten sind ausverkauft (StockQuantity = 0)
-- ============================================

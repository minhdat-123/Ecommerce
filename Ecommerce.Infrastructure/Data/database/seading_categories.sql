-- categories table
INSERT INTO Categories (Name, ParentCategoryId)
VALUES('Electronics', NULL),('Clothing', NULL),('Home', NULL),('Books', NULL),('Sports', NULL),('Toys', NULL),('Beauty', NULL),('Automotive', NULL),('Garden', NULL),('Health', NULL);
DECLARE @ElectronicsId INT = (SELECT Id FROM Categories WHERE Name = 'Electronics' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId)
VALUES
('Computers', @ElectronicsId),
('Mobile Phones', @ElectronicsId),
('Televisions', @ElectronicsId),
('Cameras', @ElectronicsId),
('Audio Devices', @ElectronicsId),
('Gaming Consoles', @ElectronicsId),
('Wearables', @ElectronicsId),
('Accessories', @ElectronicsId),
('Home Appliances', @ElectronicsId),
('Office Electronics', @ElectronicsId);

DECLARE @ClothingId INT = (SELECT Id FROM Categories WHERE Name = 'Clothing' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId)
VALUES
('Men''s Clothing', @ClothingId),
('Women''s Clothing', @ClothingId),
('Kids'' Clothing', @ClothingId),
('Shoes', @ClothingId),
('Accessories', @ClothingId),
('Sportswear', @ClothingId),
('Formal Wear', @ClothingId),
('Casual Wear', @ClothingId),
('Seasonal Clothing', @ClothingId),
('Undergarments', @ClothingId);

DECLARE @HomeId INT = (SELECT Id FROM Categories WHERE Name = 'Home' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId)
VALUES
('Furniture', @HomeId),
('Kitchenware', @HomeId),
('Bedding', @HomeId),
('Decor', @HomeId),
('Lighting', @HomeId),
('Storage', @HomeId),
('Appliances', @HomeId),
('Tools', @HomeId),
('Outdoor', @HomeId),
('Pets', @HomeId);

DECLARE @BooksId INT = (SELECT Id FROM Categories WHERE Name = 'Books' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId)
VALUES
('Fiction', @BooksId),
('Non-Fiction', @BooksId),
('Children''s Books', @BooksId),
('Textbooks', @BooksId),
('Comics', @BooksId),
('Magazines', @BooksId),
('Audiobooks', @BooksId),
('E-books', @BooksId),
('Educational', @BooksId),
('Reference', @BooksId);

DECLARE @SportsId INT = (SELECT Id FROM Categories WHERE Name = 'Sports' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId)
VALUES
('Team Sports', @SportsId),
('Individual Sports', @SportsId),
('Fitness Equipment', @SportsId),
('Outdoor Sports', @SportsId),
('Water Sports', @SportsId),
('Winter Sports', @SportsId),
('Sports Apparel', @SportsId),
('Sports Accessories', @SportsId),
('Sports Nutrition', @SportsId),
('Sports Medicine', @SportsId);

DECLARE @ToysId INT = (SELECT Id FROM Categories WHERE Name = 'Toys' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId)
VALUES
('Action Figures', @ToysId),
('Dolls', @ToysId),
('Educational Toys', @ToysId),
('Board Games', @ToysId),
('Puzzles', @ToysId),
('Outdoor Toys', @ToysId),
('Building Toys', @ToysId),
('Arts and Crafts', @ToysId),
('Electronic Toys', @ToysId),
('Stuffed Animals', @ToysId);

DECLARE @BeautyId INT = (SELECT Id FROM Categories WHERE Name = 'Beauty' AND ParentCategoryId IS NULL);

INSERT INTO Categories(Name, ParentCategoryId)
VALUES
('Skincare', @BeautyId),
('Makeup', @BeautyId),
('Haircare', @BeautyId),
('Fragrances', @BeautyId),
('Personal Care', @BeautyId),
('Men''s Grooming', @BeautyId),
('Bath and Body', @BeautyId),
('Tools and Accessories', @BeautyId),
('Natural and Organic', @BeautyId),
('Luxury Beauty', @BeautyId);

DECLARE @AutomotiveId INT = (SELECT Id FROM Categories WHERE Name = 'Automotive' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId)
VALUES
('Car Parts', @AutomotiveId),
('Motorcycle Parts', @AutomotiveId),
('Tools and Equipment', @AutomotiveId),
('Car Electronics', @AutomotiveId),
('Tires and Wheels', @AutomotiveId),
('Exterior Accessories', @AutomotiveId),
('Interior Accessories', @AutomotiveId),
('Oils and Fluids', @AutomotiveId),
('Cleaning and Detailing', @AutomotiveId),
('Safety and Security', @AutomotiveId);

DECLARE @GardenId INT = (SELECT Id FROM Categories WHERE Name = 'Garden' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId)
VALUES
('Plants', @GardenId),
('Seeds', @GardenId),
('Garden Tools', @GardenId),
('Outdoor Decor', @GardenId),
('Watering Equipment', @GardenId),
('Pest Control', @GardenId),
('Soil and Fertilizers', @GardenId),
('Garden Furniture', @GardenId),
('Greenhouses', @GardenId),
('Landscaping', @GardenId);

DECLARE @HealthId INT = (SELECT Id FROM Categories WHERE Name = 'Health' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId)
VALUES
('Vitamins and Supplements', @HealthId),
('Personal Care', @HealthId),
('Medical Supplies', @HealthId),
('Fitness Equipment', @HealthId),
('Wellness Products', @HealthId),
('Alternative Medicine', @HealthId),
('Health Monitors', @HealthId),
('First Aid', @HealthId),
('Mobility Aids', @HealthId),
('Health Books', @HealthId);

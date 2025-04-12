-- categories table
INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES('Electronics', NULL, 0),('Clothing', NULL, 0),('Home', NULL, 0),('Books', NULL, 0),('Sports', NULL, 0),('Toys', NULL, 0),('Beauty', NULL, 0),('Automotive', NULL, 0),('Garden', NULL, 0),('Health', NULL, 0);
DECLARE @ElectronicsId INT = (SELECT Id FROM Categories WHERE Name = 'Electronics' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES
('Computers', @ElectronicsId, 1),
('Mobile Phones', @ElectronicsId, 1),
('Televisions', @ElectronicsId, 1),
('Cameras', @ElectronicsId, 1),
('Audio Devices', @ElectronicsId, 1),
('Gaming Consoles', @ElectronicsId, 1),
('Wearables', @ElectronicsId, 1),
('Accessories', @ElectronicsId, 1),
('Home Appliances', @ElectronicsId, 1),
('Office Electronics', @ElectronicsId, 1);

DECLARE @ClothingId INT = (SELECT Id FROM Categories WHERE Name = 'Clothing' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES
('Men''s Clothing', @ClothingId, 1),
('Women''s Clothing', @ClothingId, 1),
('Kids'' Clothing', @ClothingId, 1),
('Shoes', @ClothingId, 1),
('Accessories', @ClothingId, 1),
('Sportswear', @ClothingId, 1),
('Formal Wear', @ClothingId, 1),
('Casual Wear', @ClothingId, 1),
('Seasonal Clothing', @ClothingId, 1),
('Undergarments', @ClothingId, 1);

DECLARE @HomeId INT = (SELECT Id FROM Categories WHERE Name = 'Home' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES
('Furniture', @HomeId, 1),
('Kitchenware', @HomeId, 1),
('Bedding', @HomeId, 1),
('Decor', @HomeId, 1),
('Lighting', @HomeId, 1),
('Storage', @HomeId, 1),
('Appliances', @HomeId, 1),
('Tools', @HomeId, 1),
('Outdoor', @HomeId, 1),
('Pets', @HomeId, 1);

DECLARE @BooksId INT = (SELECT Id FROM Categories WHERE Name = 'Books' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES
('Fiction', @BooksId, 1),
('Non-Fiction', @BooksId, 1),
('Children''s Books', @BooksId, 1),
('Textbooks', @BooksId, 1),
('Comics', @BooksId, 1),
('Magazines', @BooksId, 1),
('Audiobooks', @BooksId, 1),
('E-books', @BooksId, 1),
('Educational', @BooksId, 1),
('Reference', @BooksId, 1);

DECLARE @SportsId INT = (SELECT Id FROM Categories WHERE Name = 'Sports' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES
('Team Sports', @SportsId, 1),
('Individual Sports', @SportsId, 1),
('Fitness Equipment', @SportsId, 1),
('Outdoor Sports', @SportsId, 1),
('Water Sports', @SportsId, 1),
('Winter Sports', @SportsId, 1),
('Sports Apparel', @SportsId, 1),
('Sports Accessories', @SportsId, 1),
('Sports Nutrition', @SportsId, 1),
('Sports Medicine', @SportsId, 1);

DECLARE @ToysId INT = (SELECT Id FROM Categories WHERE Name = 'Toys' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES
('Action Figures', @ToysId, 1),
('Dolls', @ToysId, 1),
('Educational Toys', @ToysId, 1),
('Board Games', @ToysId, 1),
('Puzzles', @ToysId, 1),
('Outdoor Toys', @ToysId, 1),
('Building Toys', @ToysId, 1),
('Arts and Crafts', @ToysId, 1),
('Electronic Toys', @ToysId, 1),
('Stuffed Animals', @ToysId, 1);

DECLARE @BeautyId INT = (SELECT Id FROM Categories WHERE Name = 'Beauty' AND ParentCategoryId IS NULL);

INSERT INTO Categories(Name, ParentCategoryId, Level)
VALUES
('Skincare', @BeautyId, 1),
('Makeup', @BeautyId, 1),
('Haircare', @BeautyId, 1),
('Fragrances', @BeautyId, 1),
('Personal Care', @BeautyId, 1),
('Men''s Grooming', @BeautyId, 1),
('Bath and Body', @BeautyId, 1),
('Tools and Accessories', @BeautyId, 1),
('Natural and Organic', @BeautyId, 1),
('Luxury Beauty', @BeautyId, 1);

DECLARE @AutomotiveId INT = (SELECT Id FROM Categories WHERE Name = 'Automotive' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES
('Car Parts', @AutomotiveId, 1),
('Motorcycle Parts', @AutomotiveId, 1),
('Tools and Equipment', @AutomotiveId, 1),
('Car Electronics', @AutomotiveId, 1),
('Tires and Wheels', @AutomotiveId, 1),
('Exterior Accessories', @AutomotiveId, 1),
('Interior Accessories', @AutomotiveId, 1),
('Oils and Fluids', @AutomotiveId, 1),
('Cleaning and Detailing', @AutomotiveId, 1),
('Safety and Security', @AutomotiveId, 1);

DECLARE @GardenId INT = (SELECT Id FROM Categories WHERE Name = 'Garden' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES
('Plants', @GardenId, 1),
('Seeds', @GardenId, 1),
('Garden Tools', @GardenId, 1),
('Outdoor Decor', @GardenId, 1),
('Watering Equipment', @GardenId, 1),
('Pest Control', @GardenId, 1),
('Soil and Fertilizers', @GardenId, 1),
('Garden Furniture', @GardenId, 1),
('Greenhouses', @GardenId, 1),
('Landscaping', @GardenId, 1);

DECLARE @HealthId INT = (SELECT Id FROM Categories WHERE Name = 'Health' AND ParentCategoryId IS NULL);

INSERT INTO Categories (Name, ParentCategoryId, Level)
VALUES
('Vitamins and Supplements', @HealthId, 1),
('Personal Care', @HealthId, 1),
('Medical Supplies', @HealthId, 1),
('Fitness Equipment', @HealthId, 1),
('Wellness Products', @HealthId, 1),
('Alternative Medicine', @HealthId, 1),
('Health Monitors', @HealthId, 1),
('First Aid', @HealthId, 1),
('Mobility Aids', @HealthId, 1),
('Health Books', @HealthId, 1);

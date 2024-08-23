namespace DomainModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a category that can be part of a hierarchy, with parent and subcategory relationships.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the list of parent categories to which this category belongs.
        /// </summary>
        public List<Category> Parents { get; private set; }

        /// <summary>
        /// Gets the list of subcategories that belong to this category.
        /// </summary>
        public List<Category> Subcategories { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        public Category(string name)
        {
            this.Name = name;
            this.Parents = new List<Category>();
            this.Subcategories = new List<Category>();
        }

        /// <summary>
        /// Adds a parent category to this category.
        /// </summary>
        /// <param name="parentCategory">The parent category to add.</param>
        public void AddParent(Category parentCategory)
        {
            if (!this.Parents.Contains(parentCategory))
            {
                this.Parents.Add(parentCategory);
                parentCategory.AddSubcategory(this);
            }
        }

        /// <summary>
        /// Adds a subcategory to this category.
        /// </summary>
        /// <param name="subcategory">The subcategory to add.</param>
        public void AddSubcategory(Category subcategory)
        {
            if (!this.Subcategories.Contains(subcategory))
            {
                this.Subcategories.Add(subcategory);
                subcategory.AddParent(this);
            }
        }

        /// <summary>
        /// Returns a string representation of the category.
        /// </summary>
        /// <returns>A string that represents the current category.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}

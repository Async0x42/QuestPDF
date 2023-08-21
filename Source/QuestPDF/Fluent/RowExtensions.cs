﻿using System;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public class RowDescriptor
    {
        internal Row Row { get; } = new();

        /// <summary>
        /// Adjusts horizontal spacing between items.
        /// </summary>
        public void Spacing(float value)
        {
            Row.Spacing = value;
        }

        private IContainer Item(RowItemType type, float size = 0)
        {
            var element = new RowItem
            {
                Type = type,
                Size = size
            };
            
            Row.Items.Add(element);
            return element;
        }
        
        [Obsolete("This element has been renamed since version 2022.2. Please use the RelativeItem method.")]
        public IContainer RelativeColumn(float size = 1)
        {
            return Item(RowItemType.Relative, size);
        }
        
        [Obsolete("This element has been renamed since version 2022.2. Please use the ConstantItem method.")]
        public IContainer ConstantColumn(float size)
        {
            return Item(RowItemType.Constant, size);
        }

        /// <summary>
        /// Adds a new item to the row element. This item occupies space proportionally to other relative items.
        /// </summary>
        /// <example>
        /// For a row element with a width of 100 points that has three items (a relative item of size 1, a relative item of size 5, and a constant item of size 10 points),
        /// the items will occupy sizes of 15 points, 75 points, and 10 points respectively.
        /// </example>
        /// <returns>The container of the newly added item.</returns>
        public IContainer RelativeItem(float size = 1)
        {
            return Item(RowItemType.Relative, size);
        }
        
        /// <summary>
        /// Adds a new item to the row element with a specified constant size.
        /// </summary>
        /// <returns>The container of the newly created item.</returns>
        public IContainer ConstantItem(float size, Unit unit = Unit.Point)
        {
            return Item(RowItemType.Constant, size.ToPoints(unit));
        }

        /// <summary>
        /// Adds a new item to the row element. The size of this item adjusts based on its content.
        /// </summary>
        /// <remarks>
        /// The AutoItem requests as much horizontal space as its content requires.
        /// It doesn't adjust its size based on other items and may frequently result in a <c>DocumentLayoutException</c>.
        /// It's recommended to use this API in conjunction with the <c>MaxWidth</c> element..
        /// </remarks>
        /// <returns>The container of the newly created item.</returns>
        public IContainer AutoItem()
        {
            return Item(RowItemType.Auto);
        }
    }
    
    public static class RowExtensions
    {
        /// <summary>
        /// Draws a collection of elements horizontally.
        /// Depending on the content direction mode, elements will be drawn from left to right, or from right to left.
        /// <br />
        /// <a href="https://www.questpdf.com/api-reference/row.html">Learn more</a>
        /// </summary>
        /// <remarks>
        /// Supports paging.
        /// </remarks>
        /// <param name="handler">The action to configure the row's content.</param>
        public static void Row(this IContainer element, Action<RowDescriptor> handler)
        {
            var descriptor = new RowDescriptor();
            handler(descriptor);
            element.Element(descriptor.Row);
        }
    }
}
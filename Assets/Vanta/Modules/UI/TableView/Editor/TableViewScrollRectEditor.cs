using UnityEngine;
using UnityEditor;
using UnityEngine.UI;



namespace Vanta.UI.TableView
{

    public class TableViewScrollRectEditor
    {
        
        [MenuItem("CONTEXT/ScrollRect/Replace with TableViewScrollRect")]
        public static void ReplaceWithTableViewScrollRect(MenuCommand command)
        {
            ScrollRect scrollRect = (ScrollRect) command.context;

            var content = scrollRect.content;
            var horizontal = scrollRect.horizontal;
            var vertical = scrollRect.vertical;
            var movementType = scrollRect.movementType;
            var elasticity = scrollRect.elasticity;
            var inertia = scrollRect.inertia;
            var decelerationRate = scrollRect.decelerationRate;
            var viewport = scrollRect.viewport;
            var horizontalScrollbar = scrollRect.horizontalScrollbar;
            var vertialScrollbar = scrollRect.verticalScrollbar;

            GameObject obj = scrollRect.gameObject;
            GameObject.DestroyImmediate(scrollRect);
            
            var tableRect = obj.AddComponent<TableViewScrollRect>();
            tableRect.content = content;
            tableRect.horizontal = horizontal;
            tableRect.vertical = vertical;
            tableRect.movementType = movementType;
            tableRect.elasticity = elasticity;
            tableRect.inertia = inertia;
            tableRect.decelerationRate = decelerationRate;
            tableRect.viewport = viewport;
            tableRect.horizontalScrollbar = horizontalScrollbar;
            tableRect.verticalScrollbar = vertialScrollbar;
        }
        
    }

}

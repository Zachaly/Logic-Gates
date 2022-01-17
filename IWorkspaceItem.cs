using System.Collections.Generic;
using System.Windows;

namespace Symulator_układów_logicznych
{
    public interface IWorkspaceItem
    {
        List<VisualConnection> Connections { get; set; } // Holds visual connections of this element

        // Creates,deletes logical connection between 2 elements
        void CreateConnection(IWorkspaceItem target);
        void DeleteConnection(IWorkspaceItem target);

        void CreateVisualConnection(); // Creates visual connection between 2 elements
        void Delete(); // Removes this from workspace

        Point GetStartPoint(); // Gets a point where connection starts
        Point GetEndPoint(int conNum); // Gets a point where connection ends, based on number of current connection
    }
}

using System.Collections.Generic;

/// <summary>
/// This class represents a task manager that follows a linear order
/// and only allows a single process to execute at a time.
/// </summary>
namespace File_Patcher {
    public class LinearTaskManager {
        LinkedList<System.Delegate> taskList;
        LinkedList<object[]> taskParams;

        /// <summary>
        /// Creates a new instance of the LinearTaskManager class.
        /// </summary>
        public LinearTaskManager() {
            taskList = new LinkedList<System.Delegate>();
            taskParams = new LinkedList<object[]>();
        }

        /// <summary>
        /// Adds a delegate task to the end of the list.
        /// </summary>
        /// <param name="task">The delegate to add</param>
        /// <param name="parameters">The parameters to pass to this delegate</param>
        public void addTask(System.Delegate task, params object[] parameters) {
            taskList.AddLast(task);
            taskParams.AddLast(parameters);
        }

        /// <summary>
        /// Deletes the current task list.
        /// </summary>
        public void clear() {
            taskList.Clear();
            taskParams.Clear();
        }

        /// <summary>
        /// Executes the list of tasks.
        /// </summary>
        public void executeList() {
            // Grab the first task
            LinkedListNode<System.Delegate> task = taskList.First;
            LinkedListNode<object[]> parameters = taskParams.First;

            // Execute the tasks in order
            while (task != null) {
                task.Value.DynamicInvoke(parameters.Value);
                task = task.Next;
                parameters = parameters.Next;
            }
        }
    
    }
}
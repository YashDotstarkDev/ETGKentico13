using CMS.Scheduler;

namespace ETG.Algolia.Tasks
{
    public static class SearchTaskExecutorUtils
    {
        private const string SEARCH_TASKS_PROCESSING_SCHEDULED_TASK_NAME = "Search.Algolia.TaskExecutor";

        public static void ProcessSearchTasks()
        {
            SchedulingExecutor.ExecuteTask(TaskInfoProvider.GetTaskInfo(SEARCH_TASKS_PROCESSING_SCHEDULED_TASK_NAME, 0));
        }

        public static bool IsSearchTaskProcessingRunning()
        {
            return TaskInfoProvider.GetTaskInfo(SEARCH_TASKS_PROCESSING_SCHEDULED_TASK_NAME, 0).TaskIsRunning;
        }
    }
}
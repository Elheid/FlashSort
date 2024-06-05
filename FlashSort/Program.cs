using System;

class Sort
{
    private static double coefficient = 0.42;
    private static Func<int, double, int> countClasses = (n, coefficient) => (int)(n * coefficient);
    public static int minElement = 0;
    public static int maxElement = 0;
    public static int m = 0;

    //Считает к какому классу принадлежит число
    public static int GetClassId(int value)
    {
        return ((m - 1) * (value - minElement)) / (maxElement - minElement);
    }

    //Находит индекс следующего элемента класса
    public static int FindSwapIndex(int classId, int[] array, int[] distArr)
    {
        var swapIndex = distArr[classId - 1];
        for (var i = swapIndex; i < distArr[classId]; i++)
        {
            if (GetClassId(array[i]) != classId)
            {
                swapIndex = i;
                break;
            }
        }
        return swapIndex;
    }

    //Перегруппировывает так, чтобы классы были вместе
    public static void ArrangeClasses(int[] array, int[] distArr, int classStart, int classEnd, int id)
    {
        for (var i = classStart; i < classEnd; i++)
        {
            var classId = GetClassId(array[i]);
            while (classId != id)
            {
                var swapIndex = FindSwapIndex(classId, array, distArr);
                var temp = array[i];
                array[i] = array[swapIndex];
                array[swapIndex] = temp;
                classId = GetClassId(array[i]);
            }
        }
        return;
    }



    //FlashSort
    public static void FlashSort(int [] array) {
	
        var n = array.Length; //Размерность массива


        //Шаг 1 Ищем минимальный и максимальный элементы
        for (var i = 1; i < n; i++) {
            if (array[i] < minElement)
                minElement = array[i];
            if (array[i] > maxElement)
                maxElement = array[i];
        }
        //Минимум = максимум? Тогда массив
        //состоит из одинаковых элементов.
        //А значит, он отсортирован!
        if (minElement == maxElement)
            return;


        //Шаг 2 делим диапазон на m классов/ведер

        m = countClasses(n, coefficient); //Количество классов
       
        var distributionArr = new int[m]; //Создаём вспомогательный массив c классами

		
		//Шаг 3 Классификация
        //Заполняем массив распределения
		//Каждый элемент вспомогательного массива -
		//это количество чисел соответствующего класса
        for (var i = 0; i < n; i++) {
            var countInClass = GetClassId(array[i]);
            distributionArr[countInClass]++;
        }
		
        //Шаг 4 Считаем префиксную сумму
		//Во вспомогательном массиве каждый элемент
		//(начиная со 2-го) увеличим на величину предыдущего.
		//После этого каждый элемент вспомогательного массива
		//это индекс элемента в основном массиве на котором 
		//должны заканчиваются числа соответсвующего класса
        for (var i = 1; i < m; i++){
            distributionArr[i] += distributionArr[i - 1];
        }

        //Шаг 5 Переставляем элементы
        for (var i = 0; i < m-1; i++) 
        {
            if (i == 0)
                ArrangeClasses(array, distributionArr, i, distributionArr[i], i);
            else
                ArrangeClasses(array, distributionArr, distributionArr[i - 1], distributionArr[i], i);
        }

        //Шаг 6
        //применив которую получим
        //не почти упорядоченный массив, а полностью отсортированный массив
        InsertionSortClasses(array, distributionArr);
    }
    

    //Финальная сортировка простыми вставками
    //для сортировки каждого класса
    public static void InsertionSortClasses(int[] array, int[] distArr)
    {
        for (var i = 0; i < m; i++)
        {
            if (i == 0)
                InsertionSort(array, 0, distArr[i]);            
            else

                InsertionSort(array, distArr[i - 1], distArr[i]);
        }

    }

    //Стандартная сортировка вставками
    //Чтобы отсортировать именно, классы, нужно
    //ввести начальный и конечный индекс.
    public static int[] InsertionSort(int [] a, int start, int end)  {
		int i, j, temp;
        var result = new int[a.Length];
		for (i = start + 1; i < end; i++) {	
			temp = a[i];
			j = i - 1;
			while (j >= start && a[j] > temp) 
            {
				a[j+1] = a[j];
				j--;
			}
			a[j+1] = temp;			
		}
        return result;

    }

    //Вспомагательный метод печати массива в cmd
    static void PrintArray(int[] arr)
    {
        foreach (var item in arr)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        int[] arr = { 45, 75, 24, 20, 66, 55, 70, 38};
        Console.WriteLine("Начальный массив:");
        PrintArray(arr);
        Sort.FlashSort(arr);
        Console.WriteLine("\nОтсортированный массив:");
        PrintArray(arr);

        Console.WriteLine("Хотите отсортировать свой массив? (да/нет)");
        var answer = Console.ReadLine();
        if (answer == "да") 
        {
            Console.WriteLine("Введите свой массив, перечисляя элементы через пробел");
            var array = Console.ReadLine();
            var intArray = array.Split().Select((symbol)=> int.Parse(symbol)).ToArray();
            Sort.FlashSort(intArray);
            Console.WriteLine("\nОтсортированный массив:");
            PrintArray(intArray);
        }

    }
}

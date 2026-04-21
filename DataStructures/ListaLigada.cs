namespace SimuladorFisica.DataStructures
{
    public class ListaLigada<T>
    {
        private Node<T>? head;
        private Node<T>? tail;
        private int count;

        public int Count => count;

        public void AddLast(T value)
        {
            Node<T> newNode = new Node<T>(value);

            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail!.Next = newNode;
                tail = newNode;
            }

            count++;
        }

        public Node<T>? Head()
        {
            return head;
        }

        public List<T> ToList()
        {
            List<T> list = new List<T>();
            Node<T>? current = head;

            while (current != null)
            {
                list.Add(current.Value);
                current = current.Next;
            }

            return list;
        }

        public T? Find(Predicate<T> predicate)
        {
            Node<T>? current = head;

            while (current != null)
            {
                if (predicate(current.Value))
                    return current.Value;

                current = current.Next;
            }

            return default;
        }
    }
}
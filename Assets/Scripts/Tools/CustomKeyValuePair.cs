namespace BuffSystem
{
    [System.Serializable]
    public class CustomKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public CustomKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}


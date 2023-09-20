import axios from "axios";
import { useState, useEffect } from "react";

export default function useFetch(url) {
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    axios.get(url).then((response) => {
      if (!response.ok) {
        setData(null);
        setIsLoading(false);
        setError(response.statusText);
      }
      setData(response.data);
      setIsLoading(false);
      setError(null);
    });
  }, [url]);

  return { data, isLoading, error };
}

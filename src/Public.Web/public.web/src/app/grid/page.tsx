'use client'

import { useEffect, useState } from 'react';

export default function Home() {
  const [data, setData] = useState(null);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch('https://localhost:7052/captcha/grid');
        if (!response.ok) {
          throw new Error(`Error: ${response.status}`);
        }
        const json = await response.json();
        setData(json);
      } catch (err) {
        setError(err.message);
      }
    };

    fetchData();
  }, []);

  if (error) {
    return <div>Error: {error}</div>;
  }

  if (!data) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>Parent ID: {data.id}</h1>
      <div>
        {data.childrenAsBytes.map((byte, index) => (
          <div key={index}>
            <img src={`data:image/png;base64,${byte}`} alt={`Child ${index}`} />
          </div>
        ))}
      </div>
    </div>
  );
}
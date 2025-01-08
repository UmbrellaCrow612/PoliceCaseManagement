"use client";

import { useEffect, useState } from "react";
import Image from "next/image";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { AlertCircle, RefreshCw, CheckCircle } from 'lucide-react';
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";

interface CaptchaChild {
  id: string;
  bytes: string;
}

interface CaptchaData {
  id: string;
  text: string;
  children: CaptchaChild[];
}

interface ValidationResponse {
  success: boolean;
  message?: string;
  errors?: string[];
}

export default function CaptchaGrid() {
  const [data, setData] = useState<CaptchaData | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [selectedIds, setSelectedIds] = useState<string[]>([]);
  const [validationResult, setValidationResult] = useState<ValidationResponse | null>(null);

  const fetchData = async () => {
    setIsLoading(true);
    setError(null);
    setValidationResult(null);
    setSelectedIds([]);
    try {
      const response = await fetch("https://localhost:7052/captcha/grid");
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const json = await response.json();
      setData(json);
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "An unknown error occurred"
      );
    } finally {
      setIsLoading(false);
    }
  };

  const handleValidate = async () => {
    if (!data) return;

    try {
      const response = await fetch("https://localhost:7052/captcha/grid", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          id: data.id,
          selectedIds,
        }),
      });

      const result: ValidationResponse = await response.json();
      setValidationResult(result);
    } catch (err) {
      setValidationResult({
        success: false,
        errors: [err instanceof Error ? err.message : "Validation failed."]
      });
    }
  };

  const toggleSelection = (childId: string) => {
    setSelectedIds((prev) =>
      prev.includes(childId)
        ? prev.filter((id) => id !== childId)
        : [...prev, childId]
    );
  };

  useEffect(() => {
    fetchData();
  }, []);

  if (error) {
    return (
      <Alert variant="destructive">
        <AlertCircle className="h-4 w-4" />
        <AlertTitle>Error</AlertTitle>
        <AlertDescription>{error}</AlertDescription>
      </Alert>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Captcha Grid</h1>
        <Button onClick={fetchData} disabled={isLoading}>
          <RefreshCw className="mr-2 h-4 w-4" />
          Refresh
        </Button>
      </div>
      {isLoading ? (
        <div className="grid grid-cols-3 gap-4">
          {[...Array(9)].map((_, index) => (
            <Skeleton key={index} className="h-32 w-full" />
          ))}
        </div>
      ) : data ? (
        <>
          <p className="mb-4">
            Parent ID: {data.id} Value you need to make by picking selected
            children: {data.text}
          </p>
          <div className="grid grid-cols-3 gap-4">
            {data.children.map((child) => (
              <div
                key={child.id}
                className={`relative h-32 border-2 rounded-md ${
                  selectedIds.includes(child.id)
                    ? "border-blue-500"
                    : "border-transparent"
                } cursor-pointer`}
                onClick={() => toggleSelection(child.id)}
              >
                <Image
                  src={`data:image/png;base64,${child.bytes}`}
                  alt={`Captcha image ${child.id}`}
                  fill
                  className="rounded-md"
                />
              </div>
            ))}
          </div>
          <div className="mt-6">
            <Button
              onClick={handleValidate}
              disabled={selectedIds.length === 0}
            >
              Validate Selection
            </Button>
            {validationResult && (
              <Alert variant={validationResult.success ? "default" : "destructive"} className="mt-4">
                {validationResult.success ? (
                  <CheckCircle className="h-4 w-4" />
                ) : (
                  <AlertCircle className="h-4 w-4" />
                )}
                <AlertTitle>{validationResult.success ? "Success" : "Error"}</AlertTitle>
                <AlertDescription>
                  {validationResult.success
                    ? validationResult.message
                    : validationResult.errors?.join(", ") || "Validation failed."}
                </AlertDescription>
              </Alert>
            )}
          </div>
        </>
      ) : null}
    </div>
  );
}


import { Text, View, StyleSheet } from "react-native";
import { useLocalSearchParams } from "expo-router";
import { useEffect, useState } from "react";

export default function OTPTab() {
  const { expiresAt, code } = useLocalSearchParams<any>();
  const [remainingTime, setRemainingTime] = useState("");

  useEffect(() => {
    if (expiresAt) {
      const expirationTime = new Date(expiresAt).getTime();

      const interval = setInterval(() => {
        const now = new Date().getTime();
        const timeLeft = expirationTime - now;

        if (timeLeft > 0) {
          const minutes = Math.floor(
            (timeLeft % (1000 * 60 * 60)) / (1000 * 60)
          );
          const seconds = Math.floor((timeLeft % (1000 * 60)) / 1000);
          setRemainingTime(`${minutes}m ${seconds}s`);
        } else {
          setRemainingTime("Expired");
          clearInterval(interval);
        }
      }, 1000);

      return () => clearInterval(interval);
    }
  }, [expiresAt]);

  return (
    <View style={styles.container}>
      <Text style={styles.title}>One Time Password Details</Text>

      <View style={styles.card}>
        <View style={styles.row}>
          <Text style={styles.label}>Code:</Text>
          <Text style={[styles.value, styles.code]}>{code}</Text>
        </View>

        <View style={styles.row}>
          <Text style={styles.label}>Expires At:</Text>
          <Text style={styles.value}>{remainingTime}</Text>
        </View>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    padding: 20,
    backgroundColor: "#f5f5f5",
  },
  title: {
    fontSize: 24,
    fontWeight: "bold",
    marginBottom: 20,
    color: "#333",
  },
  card: {
    width: "100%",
    backgroundColor: "#fff",
    borderRadius: 10,
    padding: 20,
    shadowColor: "#000",
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 6,
    elevation: 3,
  },
  row: {
    flexDirection: "row",
    justifyContent: "space-between",
    marginBottom: 15,
  },
  label: {
    fontSize: 16,
    color: "#666",
    fontWeight: "500",
  },
  value: {
    fontSize: 16,
    color: "#333",
    fontWeight: "bold",
  },
  code: {
    color: "#007BFF",
    fontSize: 18,
  },
});

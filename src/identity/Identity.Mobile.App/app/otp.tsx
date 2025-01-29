import { Text, View, StyleSheet } from "react-native";
import { useLocalSearchParams } from "expo-router";

export default function OTPTab() {
  const { id, CreatedAt, ExpiresAt, Code } = useLocalSearchParams();

  return (
    <View style={styles.container}>
      <Text style={styles.title}>OTP Details</Text>
      <Text>ID: {id}</Text>
      <Text>Created At: {CreatedAt}</Text>
      <Text>Expires At: {ExpiresAt}</Text>
      <Text>Code: {Code}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    padding: 20,
  },
  title: {
    fontSize: 24,
    fontWeight: "bold",
    marginBottom: 10,
  },
});

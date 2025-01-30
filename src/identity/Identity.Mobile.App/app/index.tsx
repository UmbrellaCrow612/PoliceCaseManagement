import { useState, useCallback, useEffect } from "react";
import {
  View,
  Text,
  FlatList,
  StyleSheet,
  TouchableOpacity,
} from "react-native";
import { Link, useFocusEffect } from "expo-router";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { TOTP } from "otpauth";

interface TotpData {
  issuer: string;
  appName: string;
  userName: string;
  secret: string;
}

export default function TotpListTab() {
  const [totpList, setTotpList] = useState<TotpData[]>([]);
  const [codes, setCodes] = useState<Record<string, string>>({});

  const generateTotpCode = (secret: string) => {
    try {
      const totp = new TOTP({ secret, digits: 6 });
      return totp.generate();
    } catch (error) {
      console.error("Error generating TOTP code:", error);
      return "------";
    }
  };

  useEffect(() => {
    const updateCodes = () => {
      setCodes((prevCodes) => {
        const newCodes = { ...prevCodes };
        totpList.forEach((item) => {
          const key = `${item.issuer}-${item.userName}`;
          newCodes[key] = generateTotpCode(item.secret);
        });
        return newCodes;
      });
    };

    updateCodes();
    const interval = setInterval(updateCodes, 30000);
    return () => clearInterval(interval);
  }, [totpList]);

  useFocusEffect(
    useCallback(() => {
      let isActive = true;

      const loadTotp = async () => {
        try {
          const keysString = await AsyncStorage.getItem("totp-keys");
          if (!keysString) {
            setTotpList([]);
            return;
          }

          const keys = JSON.parse(keysString) as string[];
          const totpPromises = keys.map(async (key) => {
            const item = await AsyncStorage.getItem(key);
            return item ? (JSON.parse(item) as TotpData) : null;
          });

          const totps: any = (await Promise.all(totpPromises)).filter(Boolean);
          if (isActive) setTotpList(totps);
        } catch (error) {
          console.error("Error loading TOTPs:", error);
        }
      };

      loadTotp();
      return () => {
        isActive = false;
      };
    }, [])
  );

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.title}>Stored TOTP Codes</Text>
        <Link href="/scan" asChild>
          <TouchableOpacity style={styles.scanButton}>
            <Text style={styles.scanButtonText}>Scan</Text>
          </TouchableOpacity>
        </Link>
      </View>

      <FlatList
        data={totpList}
        style={styles.list}
        contentContainerStyle={styles.listContent}
        keyExtractor={(item) => `${item.issuer}-${item.userName}`}
        renderItem={({ item }) => (
          <View style={styles.itemContainer}>
            <View style={styles.itemContent}>
              <View style={styles.itemHeader}>
                <Text style={styles.issuer}>{item.issuer}</Text>
                <Text style={styles.appName}>{item.appName}</Text>
              </View>
              <Text style={styles.userName}>{item.userName}</Text>
              <Text style={styles.secret}>
                TOTP: {codes[`${item.issuer}-${item.userName}`] || "------"}
              </Text>
            </View>
          </View>
        )}
        ListEmptyComponent={
          <Text style={styles.emptyText}>No TOTP entries found</Text>
        }
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    backgroundColor: "#f5f5f5",
  },
  header: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: 20,
  },
  title: {
    fontSize: 20,
    fontWeight: "bold",
    color: "#333",
  },
  scanButton: {
    backgroundColor: "#007AFF",
    paddingVertical: 8,
    paddingHorizontal: 16,
    borderRadius: 8,
  },
  scanButtonText: {
    color: "white",
    fontWeight: "bold",
  },
  list: {
    flex: 1,
    width: "100%",
  },
  listContent: {
    paddingBottom: 16,
  },
  itemContainer: {
    backgroundColor: "white",
    padding: 16,
    borderRadius: 8,
    marginBottom: 12,
    elevation: 2,
    shadowColor: "#000",
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.2,
    shadowRadius: 2,
  },
  itemHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    marginBottom: 8,
  },
  issuer: {
    fontWeight: "bold",
    fontSize: 16,
    color: "#333",
  },
  appName: {
    color: "#666",
    fontSize: 14,
  },
  userName: {
    color: "#444",
    fontSize: 14,
    marginBottom: 4,
  },
  secret: {
    color: "#888",
    fontSize: 16,
    fontWeight: "bold",
  },
  emptyText: {
    textAlign: "center",
    color: "#666",
    marginTop: 20,
  },
  itemContent: {
    flex: 1,
  },
});

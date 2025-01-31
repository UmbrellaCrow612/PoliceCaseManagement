import React, { useState, useEffect } from "react";
import { View, StyleSheet, FlatList, Text, Alert, TouchableOpacity } from "react-native";
import AsyncStorage from "@react-native-async-storage/async-storage";
import TotpItem from "@/components/totp-item";
import ScanBtn from "@/components/scan-btn";

interface TotpData {
  issuer: string;
  userName: string;
  secret: string;
  appName: string;
}

export default function TotpListTab() {
  const [totpItems, setTotpItems] = useState<TotpData[]>([]);

  useEffect(() => {
    fetchTotpItems();
  }, []);

  const fetchTotpItems = async () => {
    try {
      const keysString = await AsyncStorage.getItem("totp-keys");
      if (!keysString) return;

      const keys = JSON.parse(keysString);

      const items: TotpData[] = await Promise.all(
        keys.map(async (key: string) => {
          const itemString = await AsyncStorage.getItem(key);
          return itemString ? JSON.parse(itemString) : null;
        })
      );

      setTotpItems(items.filter(Boolean));
    } catch (error) {
      console.error("Failed to fetch TOTP items:", error);
    }
  };

  const handleDeleteItem = async (item: TotpData) => {
    try {
      // Create the key used during storage
      const key = `totp-${item.issuer}-${item.userName}`;

      // Remove the specific item
      await AsyncStorage.removeItem(key);

      // Update keys list
      const keysString = await AsyncStorage.getItem("totp-keys");
      if (keysString) {
        const keys = JSON.parse(keysString);
        const updatedKeys = keys.filter((k: string) => k !== key);
        await AsyncStorage.setItem("totp-keys", JSON.stringify(updatedKeys));
      }

      // Update state to remove the item from the list
      setTotpItems(totpItems.filter(
        (i) => i.issuer !== item.issuer || i.userName !== item.userName
      ));
    } catch (error) {
      console.error("Failed to delete TOTP item:", error);
    }
  };

  const confirmDeleteItem = (item: TotpData) => {
    Alert.alert(
      "Delete TOTP Item",
      `Are you sure you want to delete the TOTP item for ${item.appName} (${item.userName})?`,
      [
        {
          text: "Cancel",
          style: "cancel"
        },
        { 
          text: "Delete", 
          style: "destructive",
          onPress: () => handleDeleteItem(item)
        }
      ]
    );
  };

  const renderTotpItem = ({ item }: { item: TotpData }) => (
    <TouchableOpacity 
      delayLongPress={500}
      onLongPress={() => confirmDeleteItem(item)}
    >
      <TotpItem
        details={{
          appName: item.appName,
          issuer: item.issuer,
          secret: item.secret,
          userName: item.userName,
        }}
      />
    </TouchableOpacity>
  );

  return (
    <View style={styles.container}>
      <View>
        <ScanBtn />
      </View>
      <FlatList
        data={totpItems}
        renderItem={renderTotpItem}
        keyExtractor={(item) => `${item.issuer}-${item.userName}`}
        contentContainerStyle={styles.listContainer}
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>No TOTP items found</Text>
          </View>
        }
        ItemSeparatorComponent={() => <View style={styles.separator} />}
        showsVerticalScrollIndicator={true}
        bounces={true}
        alwaysBounceVertical={true}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f5f5f5",
  },
  listContainer: {
    paddingTop: 16,
    paddingBottom: 24,
    paddingHorizontal: 16,
  },
  separator: {
    height: 12,
    backgroundColor: "transparent",
  },
  emptyContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    marginTop: 50,
  },
  emptyText: {
    fontSize: 16,
    color: "#888",
  },
});